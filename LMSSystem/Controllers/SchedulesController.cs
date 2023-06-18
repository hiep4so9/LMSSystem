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
    public class SchedulesController : ControllerBase
    {
        private readonly IScheduleRepository _ScheduleRepo;

        public SchedulesController(IScheduleRepository repo)
        {
            _ScheduleRepo = repo;
        }

        [HttpGet/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetAllSchedules(int page = 1, int pageSize = 10)
        {
            try
            {
                var allSchedules = await _ScheduleRepo.GetAllSchedulesAsync();
                var paginatedSchedules = Pagination.Paginate(allSchedules, page, pageSize);

                var totalSchedules = allSchedules.Count;
                var totalPages = Pagination.CalculateTotalPages(totalSchedules, pageSize);

                var paginationInfo = new
                {
                    TotalSchedules = totalSchedules,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Ok(new { Schedules = paginatedSchedules, Pagination = paginationInfo });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetScheduleById(int id)
        {
            var Schedule = await _ScheduleRepo.GetScheduleAsync(id);
            return Schedule == null ? NotFound() : Ok(Schedule);
        }

        [HttpPost/*, Authorize*/]
        public async Task<IActionResult> AddNewSchedule(ScheduleDTO model)
        {
            try
            {
                var newScheduleId = await _ScheduleRepo.AddScheduleAsync(model);
                var Schedule = await _ScheduleRepo.GetScheduleAsync(newScheduleId);
                return Schedule == null ? NotFound() : Ok(Schedule);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> UpdateSchedule(int id, [FromBody] ScheduleDTO model)
        {
            if (id != model.ScheduleID)
            {
                return NotFound();
            }
            await _ScheduleRepo.UpdateScheduleAsync(id, model);
            return Ok();
        }

        [HttpDelete("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> DeleteSchedule([FromRoute] int id)
        {
            await _ScheduleRepo.DeleteScheduleAsync(id);
            return Ok();
        }
    }
}
