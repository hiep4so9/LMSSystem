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
    public class ClassesController : ControllerBase
    {
        private readonly IClassRepository _ClassRepo;

        public ClassesController(IClassRepository repo)
        {
            _ClassRepo = repo;
        }

        [HttpGet,Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllClasses(int page = 1, int pageSize = 10, string? keyword = null)
        {
            try
            {
                var allClasses = await _ClassRepo.GetAllClassesAsync();

                // Lọc danh sách lớp dựa trên keyword nếu keyword không null
                if (!string.IsNullOrEmpty(keyword))
                {
                    allClasses = allClasses.Where(c => c.ClassName.Contains(keyword)).ToList();
                }

                var paginatedClasses = Pagination.Paginate(allClasses, page, pageSize);

                var totalClasses = allClasses.Count;
                var totalPages = Pagination.CalculateTotalPages(totalClasses, pageSize);

                var paginationInfo = new
                {
                    TotalClasses = totalClasses,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Ok(new { Classes = paginatedClasses, Pagination = paginationInfo });
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpGet("{id}"), Authorize(Roles = "Admin,Teacher,User")]
        public async Task<IActionResult> GetClassById(int id)
        {
            var Class = await _ClassRepo.GetClassAsync(id);
            return Class == null ? NotFound() : Ok(Class);
        }

        [HttpPost, Authorize(Roles = "Teacher")]
        public async Task<IActionResult> AddNewClass(ClassDTO model)
        {
            try
            {
                var newClassId = await _ClassRepo.AddClassAsync(model);
                var Class = await _ClassRepo.GetClassAsync(newClassId);
                return Class == null ? NotFound() : Ok(Class);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateClass(int id, [FromBody] ClassDTO model)
        {
            if (id != model.ClassID)
            {
                return NotFound();
            }
            await _ClassRepo.UpdateClassAsync(id, model);
            return Ok();
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteClass([FromRoute] int id)
        {
            await _ClassRepo.DeleteClassAsync(id);
            return Ok();
        }
    }
}
