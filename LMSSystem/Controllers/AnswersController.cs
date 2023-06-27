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
    public class AnswersController : ControllerBase
    {
        private readonly IAnswerRepository _AnswerRepo;

        public AnswersController(IAnswerRepository repo)
        {
            _AnswerRepo = repo;
        }

        [HttpGet/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetAllAnswers(int page = 1, int pageSize = 10, string? keyword = null)
        {
            try
            {
                var allAnswers = await _AnswerRepo.GetAllAnswersAsync();

                // Lọc danh sách câu trả lời dựa trên keyword nếu keyword không null
                if (!string.IsNullOrEmpty(keyword))
                {
                    allAnswers = allAnswers.Where(a => a.AnswerContent.Contains(keyword)).ToList();
                }

                var paginatedAnswers = Pagination.Paginate(allAnswers, page, pageSize);

                var totalAnswers = allAnswers.Count;
                var totalPages = Pagination.CalculateTotalPages(totalAnswers, pageSize);

                var paginationInfo = new
                {
                    TotalAnswers = totalAnswers,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Ok(new { Answers = paginatedAnswers, Pagination = paginationInfo });
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpGet("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetAnswerById(int id)
        {
            var Answer = await _AnswerRepo.GetAnswerAsync(id);
            return Answer == null ? NotFound() : Ok(Answer);
        }

        [HttpPost/*, Authorize*/]
        public async Task<IActionResult> AddNewAnswer(AnswerDTO model)
        {
            try
            {
                var newAnswerId = await _AnswerRepo.AddAnswerAsync(model);
                var Answer = await _AnswerRepo.GetAnswerAsync(newAnswerId);
                return Answer == null ? NotFound() : Ok(Answer);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> UpdateAnswer(int id, [FromBody] AnswerDTO model)
        {
            if (id != model.AnswerID)
            {
                return NotFound();
            }
            await _AnswerRepo.UpdateAnswerAsync(id, model);
            return Ok();
        }

        [HttpDelete("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> DeleteAnswer([FromRoute] int id)
        {
            await _AnswerRepo.DeleteAnswerAsync(id);
            return Ok();
        }
    }
}
