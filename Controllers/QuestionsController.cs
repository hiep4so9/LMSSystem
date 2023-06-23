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
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _QuestionRepo;

        public QuestionsController(IQuestionRepository repo)
        {
            _QuestionRepo = repo;
        }

        [HttpGet/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetAllQuestions(int page = 1, int pageSize = 10)
        {
            try
            {
                var allQuestions = await _QuestionRepo.GetAllQuestionsAsync();
                var paginatedQuestions = Pagination.Paginate(allQuestions, page, pageSize);

                var totalQuestions = allQuestions.Count;
                var totalPages = Pagination.CalculateTotalPages(totalQuestions, pageSize);

                var paginationInfo = new
                {
                    TotalQuestions = totalQuestions,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Ok(new { Questions = paginatedQuestions, Pagination = paginationInfo });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetQuestionById(int id)
        {
            var Question = await _QuestionRepo.GetQuestionAsync(id);
            return Question == null ? NotFound() : Ok(Question);
        }

        [HttpPost/*, Authorize*/]
        public async Task<IActionResult> AddNewQuestion(QuestionDTO model)
        {
            try
            {
                var newQuestionId = await _QuestionRepo.AddQuestionAsync(model);
                var Question = await _QuestionRepo.GetQuestionAsync(newQuestionId);
                return Question == null ? NotFound() : Ok(Question);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> UpdateQuestion(int id, [FromBody] QuestionDTO model)
        {
            if (id != model.QuestionID)
            {
                return NotFound();
            }
            await _QuestionRepo.UpdateQuestionAsync(id, model);
            return Ok();
        }

        [HttpDelete("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> DeleteQuestion([FromRoute] int id)
        {
            await _QuestionRepo.DeleteQuestionAsync(id);
            return Ok();
        }
    }
}
