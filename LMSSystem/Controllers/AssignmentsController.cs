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
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentRepository _AssignmentRepo;
        private readonly IFirebaseStorageRepository _firebaseStorageService;

        public AssignmentsController(IAssignmentRepository repo, IFirebaseStorageRepository firebaseStorageService)
        {
            _AssignmentRepo = repo;
            _firebaseStorageService = firebaseStorageService;
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAssignments(int page = 1, int pageSize = 10, string? keyword = null)
        {
            try
            {
                var allAssignments = await _AssignmentRepo.GetAllAssignmentsAsync();

                // Lọc danh sách bài tập dựa trên keyword nếu keyword không null
                if (!string.IsNullOrEmpty(keyword))
                {
                    allAssignments = allAssignments.Where(a => a.AssignmentTitle.Contains(keyword)).ToList();
                }

                var paginatedAssignments = Pagination.Paginate(allAssignments, page, pageSize);

                var totalAssignments = allAssignments.Count;
                var totalPages = Pagination.CalculateTotalPages(totalAssignments, pageSize);

                var paginationInfo = new
                {
                    TotalAssignments = totalAssignments,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Ok(new { Assignments = paginatedAssignments, Pagination = paginationInfo });
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpGet("{id}"), Authorize(Roles = "Admin,Teacher,User")]
        public async Task<IActionResult> GetAssignmentById(int id)
        {
            var Assignment = await _AssignmentRepo.GetAssignmentAsync(id);
            return Assignment == null ? NotFound() : Ok(Assignment);
        }


        [HttpGet("class/{classId}"), Authorize(Roles = "Admin,Teacher,User")]
        public async Task<IActionResult> GetAssignmentByCourseId(int classId, int page = 1, int pageSize = 10)
        {
            try
            {
                var Assignments = await _AssignmentRepo.GetAssignmentByCLassAsync(classId);
                var paginatedAssignments = Pagination.Paginate(Assignments, page, pageSize);

                var totalAssignments = Assignments.Count;
                var totalPages = Pagination.CalculateTotalPages(totalAssignments, pageSize);

                var paginationInfo = new
                {
                    TotalAssignments = totalAssignments,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Ok(new { Assignments = paginatedAssignments, Pagination = paginationInfo });
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpGet("download/{assignmentId}"), Authorize(Roles = "Admin,Teacher,User")]
        public async Task<IActionResult> DownloadAssignment(int AssignmentId)
        {
            try
            {
                var Assignment = await _AssignmentRepo.GetAssignmentAsync(AssignmentId);

                if (Assignment == null)
                {
                    return NotFound();
                }

                // Lấy tên tệp tin và tên bucket từ URL của tệp tin trên Firebase
                var fileUrl = Assignment.AssignmentFile;
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
                return File(fileData, "application/octet-stream", Assignment.AssignmentTitle);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về mã lỗi hoặc thông báo lỗi phù hợp
                return BadRequest(ex.Message);
            }
        }



        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNewAssignment(IFormFile file, int classId)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file selected.");
                }

                var fileName = Path.GetFileName(file.FileName);
                var fileUrl = await _firebaseStorageService.UploadFileAsync(file, null, "Assignment", null);

                var model = new AssignmentDTO
                {
                    AssignmentTitle = fileName,
                    AssignmentFile = fileUrl,
                    ClassID = classId,
                    Deadline = DateTime.Now
                };

                var newAssignmentId = await _AssignmentRepo.AddAssignmentAsync(model);
                var Assignment = await _AssignmentRepo.GetAssignmentAsync(newAssignmentId);

                return Assignment == null ? NotFound() : Ok(Assignment);
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAssignment(int id, IFormFile updatedFile, int classId)
        {
            try
            {
                // Kiểm tra xem vật liệu có tồn tại trong cơ sở dữ liệu hay không
                var existingAssignment = await _AssignmentRepo.GetAssignmentAsync(id);
                if (existingAssignment == null)
                {
                    return NotFound();
                }

                // Nếu có tệp tin được gửi lên từ client, thực hiện quá trình tải lên và cập nhật URL tệp tin
                string fileUrl = existingAssignment.AssignmentFile;
                if (updatedFile != null && updatedFile.Length > 0)
                {
                    var fileName = Path.GetFileName(updatedFile.FileName);
                    fileUrl = await _firebaseStorageService.UploadFileAsync(updatedFile, null, "Assignment", null);

                    // Cập nhật thông tin vật liệu
                    existingAssignment.AssignmentTitle = fileName;
                    existingAssignment.AssignmentFile = fileUrl;
                }

                existingAssignment.ClassID = classId;

                await _AssignmentRepo.UpdateAssignmentAsync(id, existingAssignment);

                return Ok(existingAssignment);
            }
            catch
            {
                return BadRequest();
            }
        }





        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAssignment([FromRoute] int id)
        {
            try
            {
                var Assignment = await _AssignmentRepo.GetAssignmentAsync(id);

                if (Assignment == null)
                {
                    return NotFound();
                }

                // Lấy tên tệp tin và tên bucket từ URL của tệp tin trên Firebase
                var fileUrl = Assignment.AssignmentFile;
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
