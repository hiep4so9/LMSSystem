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
    public class ExamsController : ControllerBase
    {
        private readonly IExamRepository _ExamRepo;

        public ExamsController(IExamRepository repo)
        {
            _ExamRepo = repo;
        }

        [HttpGet/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetAllExams(int page = 1, int pageSize = 10)
        {
            try
            {
                var allExams = await _ExamRepo.GetAllExamsAsync();
                var paginatedExams = Pagination.Paginate(allExams, page, pageSize);

                var totalExams = allExams.Count;
                var totalPages = Pagination.CalculateTotalPages(totalExams, pageSize);

                var paginationInfo = new
                {
                    TotalExams = totalExams,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Ok(new { Exams = paginatedExams, Pagination = paginationInfo });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetExamById(int id)
        {
            var Exam = await _ExamRepo.GetExamAsync(id);
            return Exam == null ? NotFound() : Ok(Exam);
        }

        [HttpPost/*, Authorize*/]
        public async Task<IActionResult> AddNewExam(ExamDTO model)
        {
            try
            {
                var newExamId = await _ExamRepo.AddExamAsync(model);
                var Exam = await _ExamRepo.GetExamAsync(newExamId);
                return Exam == null ? NotFound() : Ok(Exam);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> UpdateExam(int id, [FromBody] ExamDTO model)
        {
            if (id != model.ExamID)
            {
                return NotFound();
            }
            await _ExamRepo.UpdateExamAsync(id, model);
            return Ok();
        }

        [HttpDelete("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> DeleteExam([FromRoute] int id)
        {
            await _ExamRepo.DeleteExamAsync(id);
            return Ok();
        }
    }
}
