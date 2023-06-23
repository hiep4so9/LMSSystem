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
    public class LessonsController : ControllerBase
    {
        private readonly ILessonRepository _LessonRepo;

        public LessonsController(ILessonRepository repo)
        {
            _LessonRepo = repo;
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

        [HttpPost/*, Authorize*/]
        public async Task<IActionResult> AddNewLesson(LessonDTO model)
        {
            try
            {
                var newLessonId = await _LessonRepo.AddLessonAsync(model);
                var Lesson = await _LessonRepo.GetLessonAsync(newLessonId);
                return Lesson == null ? NotFound() : Ok(Lesson);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> UpdateLesson(int id, [FromBody] LessonDTO model)
        {
            if (id != model.LessonID)
            {
                return NotFound();
            }
            await _LessonRepo.UpdateLessonAsync(id, model);
            return Ok();
        }

        [HttpDelete("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> DeleteLesson([FromRoute] int id)
        {
            await _LessonRepo.DeleteLessonAsync(id);
            return Ok();
        }
    }
}
