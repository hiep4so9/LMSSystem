using AutoMapper;
using LMSSystem.Data;
using LMSSystem.Models;
using LMSSystem.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LMSSystem.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly SchoolContext _context;
        private readonly IMapper _mapper;

        public AnswerRepository(SchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddAnswerAsync(AnswerDTO model)
        {
            var newAnswer = _mapper.Map<Answer>(model);
            _context.Answers!.Add(newAnswer);
            await _context.SaveChangesAsync();

            return newAnswer.AnswerID;
        }

        public async Task DeleteAnswerAsync(int id)
        {
            var deleteAnswer = _context.Answers!.SingleOrDefault(b => b.AnswerID == id);
            if (deleteAnswer != null)
            {
                _context.Answers!.Remove(deleteAnswer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<AnswerDTO>> GetAllAnswersAsync()
        {
            var Answers = await _context.Answers!.ToListAsync();
            return _mapper.Map<List<AnswerDTO>>(Answers);
        }

        public async Task<AnswerDTO> GetAnswerAsync(int id)
        {
            var Answers = await _context.Answers!.FindAsync(id);
            return _mapper.Map<AnswerDTO>(Answers);
        }

        public async Task UpdateAnswerAsync(int id, AnswerDTO model)
        {
            if (id == model.AnswerID)
            {
                var updateAnswer = _mapper.Map<Answer>(model);
                _context.Answers!.Update(updateAnswer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
