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
        public async Task<IActionResult> GetAllClasses(int page = 1, int pageSize = 10)
        {
            try
            {
                var allClasss = await _ClassRepo.GetAllClassesAsync();
                var paginatedClasss = Pagination.Paginate(allClasss, page, pageSize);

                var totalClasss = allClasss.Count;
                var totalPages = Pagination.CalculateTotalPages(totalClasss, pageSize);

                var paginationInfo = new
                {
                    TotalClasss = totalClasss,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Ok(new { Classs = paginatedClasss, Pagination = paginationInfo });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetClassById(int id)
        {
            var Class = await _ClassRepo.GetClassAsync(id);
            return Class == null ? NotFound() : Ok(Class);
        }

        [HttpPost/*, Authorize*/]
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

        [HttpPut("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> UpdateClass(int id, [FromBody] ClassDTO model)
        {
            if (id != model.ClassID)
            {
                return NotFound();
            }
            await _ClassRepo.UpdateClassAsync(id, model);
            return Ok();
        }

        [HttpDelete("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> DeleteClass([FromRoute] int id)
        {
            await _ClassRepo.DeleteClassAsync(id);
            return Ok();
        }
    }
}
