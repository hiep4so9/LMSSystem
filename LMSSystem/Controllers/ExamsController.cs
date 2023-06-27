using LMSSystem.Data;
using LMSSystem.Helpers;
using LMSSystem.Models;
using LMSSystem.Repositories;
using LMSSystem.Repositories.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Drawing.Printing;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace LMSSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly IExamRepository _ExamRepo;
        private readonly IFirebaseStorageRepository _firebaseStorageService;

        public ExamsController(IExamRepository repo, IFirebaseStorageRepository firebaseStorageService)
        {
            _ExamRepo = repo;
            _firebaseStorageService = firebaseStorageService;
        }

        [HttpGet, Authorize(Roles = "Admin,Teacher,User")]
        public async Task<IActionResult> GetAllExams(int page = 1, int pageSize = 10, string? keyword = null)
        {
            try
            {
                var allExams = await _ExamRepo.GetAllExamsAsync();
                if (!string.IsNullOrEmpty(keyword))
                {
                    allExams = allExams.Where(e => e.ExamTitle.Contains(keyword)).ToList();
                }
                var paginatedExams = Pagination.Paginate(allExams, page, pageSize);

                var totalExams = allExams.Count;
                var totalPages = Pagination.CalculateTotalPages(totalExams, pageSize);

                var paginationInfo = new
                {
                    TotalExams = totalExams,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Ok(new { Exams = paginatedExams, Pagination = paginationInfo });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}"), Authorize(Roles = "Admin,Teacher,User")]
        public async Task<IActionResult> GetExamById(int id)
        {
            var Exam = await _ExamRepo.GetExamAsync(id);
            return Exam == null ? NotFound() : Ok(Exam);
        }


        [HttpGet("course/{courseId}"), Authorize(Roles = "Admin,Teacher,User")]
        public async Task<IActionResult> GetExamByCourseId(int courseId, int page = 1, int pageSize = 10)
        {
            try
            {
                var Exams = await _ExamRepo.GetExamByCourseAsync(courseId);
                var paginatedExams = Pagination.Paginate(Exams, page, pageSize);

                var totalExams = Exams.Count;
                var totalPages = Pagination.CalculateTotalPages(totalExams, pageSize);

                var paginationInfo = new
                {
                    TotalExams = totalExams,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Ok(new { Exams = paginatedExams, Pagination = paginationInfo });
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpGet("download/{ExamId}"), Authorize(Roles = "Admin,Teacher,User")]
        public async Task<IActionResult> DownloadExam(int ExamId)
        {
            try
            {
                var Exam = await _ExamRepo.GetExamAsync(ExamId);

                if (Exam == null)
                {
                    return NotFound();
                }

                // Lấy tên tệp tin và tên bucket từ URL của tệp tin trên Firebase
                var fileUrl = Exam.ExamTitle;
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
                return File(fileData, "application/octet-stream", Exam.ExamTitle);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về mã lỗi hoặc thông báo lỗi phù hợp
                return BadRequest(ex.Message);
            }
        }



        [HttpPost, Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> AddNewExam(IFormFile file, int courseId)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file selected.");
                }

                var fileName = Path.GetFileName(file.FileName);
                var fileUrl = await _firebaseStorageService.UploadFileAsync(file, null, "Exam", null);

                var model = new ExamDTO
                {
                    ExamTitle = fileUrl,
                    CourseID = courseId,
                    ExamType = "Trắc nghiệm",
                    Duration = TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(30))
                };

                var newExamId = await _ExamRepo.AddExamAsync(model);
                var Exam = await _ExamRepo.GetExamAsync(newExamId);

                return Exam == null ? NotFound() : Ok(Exam);
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpPut("{id}"), Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> UpdateExam(int id, IFormFile updatedFile, int courseId)
        {
            try
            {
                // Kiểm tra xem vật liệu có tồn tại trong cơ sở dữ liệu hay không
                var existingExam = await _ExamRepo.GetExamAsync(id);
                if (existingExam == null)
                {
                    return NotFound();
                }

                // Nếu có tệp tin được gửi lên từ client, thực hiện quá trình tải lên và cập nhật URL tệp tin
                string fileUrl = existingExam.ExamTitle;
                if (updatedFile != null && updatedFile.Length > 0)
                {
                    var fileName = Path.GetFileName(updatedFile.FileName);
                    fileUrl = await _firebaseStorageService.UploadFileAsync(updatedFile, null, "Exam", null);

                    // Cập nhật thông tin vật liệu
                    existingExam.ExamTitle = fileUrl;
                }

                existingExam.CourseID = courseId;

                await _ExamRepo.UpdateExamAsync(id, existingExam);

                return Ok(existingExam);
            }
            catch
            {
                return BadRequest();
            }
        }





        [HttpDelete("{id}"), Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> DeleteExam([FromRoute] int id)
        {
            try
            {
                var Exam = await _ExamRepo.GetExamAsync(id);

                if (Exam == null)
                {
                    return NotFound();
                }

                // Lấy tên tệp tin và tên bucket từ URL của tệp tin trên Firebase
                var fileUrl = Exam.ExamTitle;
                Uri uri = new Uri(fileUrl);
                string bucketName = uri.Segments[3].TrimEnd('/');
                string objectName = Uri.UnescapeDataString(uri.Segments[5]);

                // Xóa tệp tin từ Firebase
                await _firebaseStorageService.DeleteFileAsync(bucketName, objectName);

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