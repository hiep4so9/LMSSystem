using AutoMapper;
using LMSSystem.Data;
using LMSSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LMSSystem.Repositories.IRepository
{
    public class LessonRepository : ILessonRepository
    {
        private readonly SchoolContext _context;
        private readonly IMapper _mapper;

        public LessonRepository(SchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddLessonAsync(LessonDTO model)
        {
            var newLesson = _mapper.Map<Lesson>(model);
            _context.Lessons!.Add(newLesson);
            await _context.SaveChangesAsync();

            return newLesson.LessonID;
        }

        public async Task DeleteLessonAsync(int id)
        {
            var deleteLesson = _context.Lessons!.SingleOrDefault(b => b.LessonID == id);
            if (deleteLesson != null)
            {
                _context.Lessons!.Remove(deleteLesson);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<LessonDTO>> GetAllLessonsAsync()
        {
            var Lessons = await _context.Lessons!.ToListAsync();
            return _mapper.Map<List<LessonDTO>>(Lessons);
        }

        public async Task<LessonDTO> GetLessonAsync(int id)
        {
            var Lessons = await _context.Lessons!.FindAsync(id);
            return _mapper.Map<LessonDTO>(Lessons);
        }

        public async Task<List<LessonDTO>> GetLessonByCourseAsync(int id)
        {
            var Lessons = await _context.Lessons
                .Where(m => m.CourseID == id)
                .ToListAsync();

            return _mapper.Map<List<LessonDTO>>(Lessons);
        }

        public async Task UpdateLessonAsync(int id, LessonDTO model)
        {
            var updateLessonl = await _context.Lessons.FindAsync(id);
            if (updateLessonl == null)
            {
                return;
            }

            _mapper.Map(model, updateLessonl);
            await _context.SaveChangesAsync();
        }
    }
}
