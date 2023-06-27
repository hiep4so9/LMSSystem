using LMSSystem.Data;
using LMSSystem.Helpers;
using LMSSystem.Models;
using LMSSystem.Repositories;
using LMSSystem.Repositories.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace LMSSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _QuestionRepo;
        private readonly IFirebaseStorageRepository _firebaseStorageService;

        public QuestionsController(IQuestionRepository repo, IFirebaseStorageRepository firebaseStorageService)
        {
            _QuestionRepo = repo;
            _firebaseStorageService = firebaseStorageService;
        }

        [HttpGet, Authorize(Roles = "Admin,Teacher,User")]
        public async Task<IActionResult> GetAllQuestions(int page = 1, int pageSize = 10, string? keyword = null)
        {
            try
            {
                var allQuestions = await _QuestionRepo.GetAllQuestionsAsync();
                if (!string.IsNullOrEmpty(keyword))
                {
                    allQuestions = allQuestions.Where(u => u.QuestionContent.Contains(keyword)).ToList();
                }
                var paginatedQuestions = Pagination.Paginate(allQuestions, page, pageSize);

                var totalQuestions = allQuestions.Count;
                var totalPages = Pagination.CalculateTotalPages(totalQuestions, pageSize);

                var paginationInfo = new
                {
                    TotalQuestions = totalQuestions,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Ok(new { Questions = paginatedQuestions, Pagination = paginationInfo });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}"), Authorize(Roles = "Admin,Teacher,User")]
        public async Task<IActionResult> GetQuestionById(int id)
        {
            var Question = await _QuestionRepo.GetQuestionAsync(id);
            return Question == null ? NotFound() : Ok(Question);
        }


        [HttpGet("exam/{examId}"), Authorize(Roles = "Admin,Teacher,User")]
        public async Task<IActionResult> GetQuestionByExamId(int examId, int page = 1, int pageSize = 10)
        {
            try
            {
                var Questions = await _QuestionRepo.GetQuestionByExamAsync(examId);
                var paginatedQuestions = Pagination.Paginate(Questions, page, pageSize);

                var totalQuestions = Questions.Count;
                var totalPages = Pagination.CalculateTotalPages(totalQuestions, pageSize);

                var paginationInfo = new
                {
                    TotalQuestions = totalQuestions,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Ok(new { Questions = paginatedQuestions, Pagination = paginationInfo });
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpGet("download/{QuestionId}"), Authorize(Roles = "Admin,Teacher,User")]
        public async Task<IActionResult> DownloadQuestion(int QuestionId)
        {
            try
            {
                var Question = await _QuestionRepo.GetQuestionAsync(QuestionId);

                if (Question == null)
                {
                    return NotFound();
                }

                // Lấy tên tệp tin và tên bucket từ URL của tệp tin trên Firebase
                var fileUrl = Question.QuestionContent;
                Uri uri = new Uri(fileUrl);
                string bucketName = uri.Segments[3].TrimEnd('/');
                string objectName = Uri.UnescapeDataString(uri.Segments[5]);

                // Tải xuống tệp tin từ Firebase
                var fileData = await _firebaseStorageService.DownloadFileAsync(bucketName, objectName);

                if (fileData == null || fileData.Length == 0)
                {
                    return NotFound();
                }

                // Trả về tệp tin đã tải xuống
                return File(fileData, "application/octet-stream", Question.QuestionContent);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về mã lỗi hoặc thông báo lỗi phù hợp
                return BadRequest(ex.Message);
            }
        }


        [HttpPost, Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> AddNewQuestion(IFormFile file, int examId)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file selected.");
                }

                var fileName = Path.GetFileName(file.FileName);
                var fileUrl = await _firebaseStorageService.UploadFileAsync(file, null, "Exam", "Question");

                var model = new QuestionDTO
                {
                    QuestionContent = fileUrl,
                    ExamID = examId
                };

                var newQuestionId = await _QuestionRepo.AddQuestionAsync(model);
                var Question = await _QuestionRepo.GetQuestionAsync(newQuestionId);

                return Question == null ? NotFound() : Ok(Question);
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpPut("{id}"), Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> UpdateQuestion(int id, IFormFile updatedFile, int examId)
        {
            try
            {
                // Kiểm tra xem vật liệu có tồn tại trong cơ sở dữ liệu hay không
                var existingQuestion = await _QuestionRepo.GetQuestionAsync(id);
                if (existingQuestion == null)
                {
                    return NotFound();
                }

                // Nếu có tệp tin được gửi lên từ client, thực hiện quá trình tải lên và cập nhật URL tệp tin
                string fileUrl = existingQuestion.QuestionContent;
                if (updatedFile != null && updatedFile.Length > 0)
                {
                    var fileName = Path.GetFileName(updatedFile.FileName);
                    fileUrl = await _firebaseStorageService.UploadFileAsync(updatedFile, null, "Exam", "Question");

                    // Cập nhật thông tin vật liệu
                    existingQuestion.QuestionContent = fileUrl;
                }

                existingQuestion.ExamID = examId;

                await _QuestionRepo.UpdateQuestionAsync(id, existingQuestion);

                return Ok(existingQuestion);
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpDelete("{id}"), Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> DeleteQuestion([FromRoute] int id)
        {
            try
            {
                var Question = await _QuestionRepo.GetQuestionAsync(id);

                if (Question == null)
                {
                    return NotFound();
                }

                // Lấy tên tệp tin và tên bucket từ URL của tệp tin trên Firebase
                var fileUrl = Question.QuestionContent;
                Uri uri = new Uri(fileUrl);
                string bucketName = uri.Segments[3].TrimEnd('/');
                string objectName = Uri.UnescapeDataString(uri.Segments[5]);

                // Xóa tệp tin từ Firebase
                await _firebaseStorageService.DeleteFileAsync(bucketName, objectName);
                await _QuestionRepo.DeleteQuestionAsync(id);

                // Thực hiện các xử lý khác sau khi xóa tệp tin thành công (nếu cần)

                return BadRequest("Delete success"); // Trả về Delete success khi xóa thành công
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về mã lỗi hoặc thông báo lỗi phù hợp
                return BadRequest(ex.Message);
            }
        }
    }
}