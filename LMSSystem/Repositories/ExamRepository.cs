using AutoMapper;
using LMSSystem.Data;
using LMSSystem.Models;
using LMSSystem.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LMSSystem.Repositories
{
    public class ExamRepository : IExamRepository
    {
        private readonly SchoolContext _context;
        private readonly IMapper _mapper;

        public ExamRepository(SchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddExamAsync(ExamDTO model)
        {
            var newExam = _mapper.Map<Exam>(model);
            _context.Exams!.Add(newExam);
            await _context.SaveChangesAsync();

            return newExam.ExamID;
        }

        public async Task DeleteExamAsync(int id)
        {
            var deleteExam = _context.Exams!.SingleOrDefault(b => b.ExamID == id);
            if (deleteExam != null)
            {
                _context.Exams!.Remove(deleteExam);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ExamDTO>> GetAllExamsAsync()
        {
            var Exams = await _context.Exams!.ToListAsync();
            return _mapper.Map<List<ExamDTO>>(Exams);
        }

        public async Task<ExamDTO> GetExamAsync(int id)
        {
            var Exams = await _context.Exams!.FindAsync(id);
            return _mapper.Map<ExamDTO>(Exams);
        }

        public async Task<List<ExamDTO>> GetExamByCourseAsync(int id)
        {
            var Exams = await _context.Exams
                .Where(m => m.CourseID == id)
                .ToListAsync();

            return _mapper.Map<List<ExamDTO>>(Exams);
        }

        public async Task UpdateExamAsync(int id, ExamDTO model)
        {
            if (id == model.ExamID)
            {
                var updateExam = _mapper.Map<Exam>(model);
                _context.Exams!.Update(updateExam);
                await _context.SaveChangesAsync();
            }
        }
    }
}
