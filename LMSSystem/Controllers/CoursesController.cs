using LMSSystem.Data;
using LMSSystem.Helpers;
using LMSSystem.Repositories.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMSSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository _CourseRepo;

        public CoursesController(ICourseRepository repo)
        {
            _CourseRepo = repo;
        }

        [HttpGet/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetAllCourses(int page = 1, int pageSize = 10, string? keyword = null)
        {
            try
            {
                var allCourses = await _CourseRepo.GetAllCoursesAsync();

                // Lọc danh sách khóa học dựa trên keyword nếu keyword không null
                if (!string.IsNullOrEmpty(keyword))
                {
                    allCourses = allCourses.Where(c => c.CourseName.Contains(keyword)).ToList();
                }

                var paginatedCourses = Pagination.Paginate(allCourses, page, pageSize);

                var totalCourses = allCourses.Count;
                var totalPages = Pagination.CalculateTotalPages(totalCourses, pageSize);

                var paginationInfo = new
                {
                    TotalCourses = totalCourses,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Ok(new { Courses = paginatedCourses, Pagination = paginationInfo });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var Course = await _CourseRepo.GetCourseAsync(id);
            return Course == null ? NotFound() : Ok(Course);
        }

        [HttpPost/*, Authorize*/]
        public async Task<IActionResult> AddNewCourse(CourseDTO model)
        {
            try
            {
                var newCourseId = await _CourseRepo.AddCourseAsync(model);
                var Course = await _CourseRepo.GetCourseAsync(newCourseId);
                return Course == null ? NotFound() : Ok(Course);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseDTO model)
        {
            if (id != model.CourseID)
            {
                return NotFound();
            }
            await _CourseRepo.UpdateCourseAsync(id, model);
            return Ok();
        }

        [HttpDelete("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> DeleteCourse([FromRoute] int id)
        {
            await _CourseRepo.DeleteCourseAsync(id);
            return Ok();
        }
    }
}
