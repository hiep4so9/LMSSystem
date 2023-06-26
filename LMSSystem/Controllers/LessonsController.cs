using LMSSystem.Data;
using LMSSystem.Helpers;
using LMSSystem.Models;
using LMSSystem.Repositories.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace LMSSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonRepository _LessonRepo;
        private readonly IFirebaseStorageRepository _firebaseStorageService;

        public LessonsController(ILessonRepository repo, IFirebaseStorageRepository firebaseStorageService)
        {
            _LessonRepo = repo;
            _firebaseStorageService = firebaseStorageService;
        }

        [HttpGet/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetAllLessons(int page = 1, int pageSize = 10)
        {
            try
            {
                var allLessons = await _LessonRepo.GetAllLessonsAsync();
                var paginatedLessons = Pagination.Paginate(allLessons, page, pageSize);

                var totalLessons = allLessons.Count;
                var totalPages = Pagination.CalculateTotalPages(totalLessons, pageSize);

                var paginationInfo = new
                {
                    TotalLessons = totalLessons,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Ok(new { Lessons = paginatedLessons, Pagination = paginationInfo });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetLessonById(int id)
        {
            var Lesson = await _LessonRepo.GetLessonAsync(id);
            return Lesson == null ? NotFound() : Ok(Lesson);
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetLessonByCourseId(int courseId, int page = 1, int pageSize = 10)
        {
            try
            {
                var Lessons = await _LessonRepo.GetLessonByCourseAsync(courseId);
                var paginatedLessons = Pagination.Paginate(Lessons, page, pageSize);

                var totalLessons = Lessons.Count;
                var totalPages = Pagination.CalculateTotalPages(totalLessons, pageSize);

                var paginationInfo = new
                {
                    TotalLessons = totalLessons,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Ok(new { Lessons = paginatedLessons, Pagination = paginationInfo });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("download/{LessonId}")]
        public async Task<IActionResult> DownloadLesson(int LessonId)
        {
            try
            {
                var Lesson = await _LessonRepo.GetLessonAsync(LessonId);

                if (Lesson == null)
                {
                    return NotFound();
                }

                // Lấy tên tệp tin và tên bucket từ URL của tệp tin trên Firebase
                var fileUrl = Lesson.LessonContent;
                Uri uri = new Uri(fileUrl);
                string bucketName = uri.Segments[3].TrimEnd('/');
                string objectName = Uri.UnescapeDataString(uri.Segments[5]);

                // Tải xuống tệp tin từ Firebase
                var fileData = await _firebaseStorageService.DownloadFileAsync(bucketName, objectName);

                // Trả về tệp tin đã tải xuống
                return File(fileData, "application/octet-stream", Lesson.LessonTitle);
            }
            catch
            {
                return BadRequest();
            }
        }




        [HttpPost]
        public async Task<IActionResult> AddNewLesson(IFormFile file, int courseId)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file selected.");
                }

                var fileName = Path.GetFileName(file.FileName);
                var fileUrl = await _firebaseStorageService.UploadFileAsync(file, null, "Lesson");

                var model = new LessonDTO
                {
                    LessonTitle = fileName,
                    LessonContent = fileUrl,
                    CourseID = courseId,
                };

                var newLessonId = await _LessonRepo.AddLessonAsync(model);
                var Lesson = await _LessonRepo.GetLessonAsync(newLessonId);

                return Lesson == null ? NotFound() : Ok(Lesson);
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLesson(int id, IFormFile updatedFile, int courseId)
        {
            try
            {
                // Kiểm tra xem vật liệu có tồn tại trong cơ sở dữ liệu hay không
                var existingLesson = await _LessonRepo.GetLessonAsync(id);
                if (existingLesson == null)
                {
                    return NotFound();
                }

                // Nếu có tệp tin được gửi lên từ client, thực hiện quá trình tải lên và cập nhật URL tệp tin
                string fileUrl = existingLesson.LessonContent;
                if (updatedFile != null && updatedFile.Length > 0)
                {
                    var fileName = Path.GetFileName(updatedFile.FileName);
                    fileUrl = await _firebaseStorageService.UploadFileAsync(updatedFile, null, "Lesson");

                    // Cập nhật thông tin vật liệu
                    existingLesson.LessonTitle = fileName;
                    existingLesson.LessonContent = fileUrl;
                }

                existingLesson.CourseID = courseId;

                await _LessonRepo.UpdateLessonAsync(id, existingLesson);

                return Ok(existingLesson);
            }
            catch
            {
                return BadRequest();
            }
        }





        [HttpDelete("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> DeleteLesson([FromRoute] int id)
        {
            try
            {
                var Lesson = await _LessonRepo.GetLessonAsync(id);

                if (Lesson == null)
                {
                    return NotFound();
                }

                // Lấy tên tệp tin và tên bucket từ URL của tệp tin trên Firebase
                var fileUrl = Lesson.LessonContent;
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
