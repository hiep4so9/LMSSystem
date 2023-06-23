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
    public class AnnouncementsController : ControllerBase
    {
        private readonly IAnnouncementRepository _AnnouncementRepo;

        public AnnouncementsController(IAnnouncementRepository repo)
        {
            _AnnouncementRepo = repo;
        }

        [HttpGet/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetAllAnnouncements(int page = 1, int pageSize = 10)
        {
            try
            {
                var allAnnouncements = await _AnnouncementRepo.GetAllAnnouncementsAsync();
                var paginatedAnnouncements = Pagination.Paginate(allAnnouncements, page, pageSize);

                var totalAnnouncements = allAnnouncements.Count;
                var totalPages = Pagination.CalculateTotalPages(totalAnnouncements, pageSize);

                var paginationInfo = new
                {
                    TotalAnnouncements = totalAnnouncements,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Ok(new { Announcements = paginatedAnnouncements, Pagination = paginationInfo });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetAnnouncementById(int id)
        {
            var Announcement = await _AnnouncementRepo.GetAnnouncementAsync(id);
            return Announcement == null ? NotFound() : Ok(Announcement);
        }

        [HttpPost/*, Authorize*/]
        public async Task<IActionResult> AddNewAnnouncement(AnnouncementDTO model)
        {
            try
            {
                var newAnnouncementId = await _AnnouncementRepo.AddAnnouncementAsync(model);
                var Announcement = await _AnnouncementRepo.GetAnnouncementAsync(newAnnouncementId);
                return Announcement == null ? NotFound() : Ok(Announcement);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> UpdateAnnouncement(int id, [FromBody] AnnouncementDTO model)
        {
            if (id != model.AnnouncementID)
            {
                return NotFound();
            }
            await _AnnouncementRepo.UpdateAnnouncementAsync(id, model);
            return Ok();
        }

        [HttpDelete("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> DeleteAnnouncement([FromRoute] int id)
        {
            await _AnnouncementRepo.DeleteAnnouncementAsync(id);
            return Ok();
        }
    }
}
