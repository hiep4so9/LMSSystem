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
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _RoleRepo;

        public RolesController(IRoleRepository repo)
        {
            _RoleRepo = repo;
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllRoles(int page = 1, int pageSize = 10, string? keyword = null)
        {
            try
            {
                var allRoles = await _RoleRepo.GetAllRolesAsync();
                if (!string.IsNullOrEmpty(keyword))
                {
                    allRoles = allRoles.Where(u => u.RoleName.Contains(keyword)).ToList();
                }
                var paginatedRoles = Pagination.Paginate(allRoles, page, pageSize);

                var totalRoles = allRoles.Count;
                var totalPages = Pagination.CalculateTotalPages(totalRoles, pageSize);

                var paginationInfo = new
                {
                    TotalRoles = totalRoles,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Ok(new { Roles = paginatedRoles, Pagination = paginationInfo });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var Role = await _RoleRepo.GetRoleAsync(id);
            return Role == null ? NotFound() : Ok(Role);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNewRole(RoleDTO model)
        {
            try
            {
                var newRoleId = await _RoleRepo.AddRoleAsync(model);
                var Role = await _RoleRepo.GetRoleAsync(newRoleId);
                return Role == null ? NotFound() : Ok(Role);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleDTO model)
        {
            if (id != model.RoleID)
            {
                return NotFound();
            }
            await _RoleRepo.UpdateRoleAsync(id, model);
            return Ok();
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole([FromRoute] int id)
        {
            await _RoleRepo.DeleteRoleAsync(id);
            return Ok();
        }
    }
}
