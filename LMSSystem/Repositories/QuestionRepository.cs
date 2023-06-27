using AutoMapper;
using LMSSystem.Data;
using LMSSystem.Models;
using LMSSystem.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LMSSystem.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly SchoolContext _context;
        private readonly IMapper _mapper;

        public QuestionRepository(SchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddQuestionAsync(QuestionDTO model)
        {
            var newQuestion = _mapper.Map<Question>(model);
            _context.Questions!.Add(newQuestion);
            await _context.SaveChangesAsync();

            return newQuestion.QuestionID;
        }

        public async Task DeleteQuestionAsync(int id)
        {
            var deleteQuestion = _context.Questions!.SingleOrDefault(b => b.QuestionID == id);
            if (deleteQuestion != null)
            {
                _context.Questions!.Remove(deleteQuestion);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<QuestionDTO>> GetAllQuestionsAsync()
        {
            var Questions = await _context.Questions!.ToListAsync();
            return _mapper.Map<List<QuestionDTO>>(Questions);
        }

        public async Task<QuestionDTO> GetQuestionAsync(int id)
        {
            var Questions = await _context.Questions!.FindAsync(id);
            return _mapper.Map<QuestionDTO>(Questions);
        }

        public async Task<List<QuestionDTO>> GetQuestionByExamAsync(int id)
        {
            var Questions = await _context.Questions
                .Where(m => m.ExamID == id)
                .ToListAsync();

            return _mapper.Map<List<QuestionDTO>>(Questions);
        }

        public async Task UpdateQuestionAsync(int id, QuestionDTO model)
        {
            if (id == model.QuestionID)
            {
                var updateQuestion = _mapper.Map<Question>(model);
                _context.Questions!.Update(updateQuestion);
                await _context.SaveChangesAsync();
            }
        }
    }
}
