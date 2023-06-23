using AutoMapper;
using LMSSystem.Data;
using LMSSystem.Models;
using LMSSystem.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LMSSystem.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly SchoolContext _context;
        private readonly IMapper _mapper;

        public CourseRepository(SchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddCourseAsync(CourseDTO model)
        {
            var newCourse = _mapper.Map<Course>(model);
            _context.Courses!.Add(newCourse);
            await _context.SaveChangesAsync();

            return newCourse.CourseID;
        }

        public async Task DeleteCourseAsync(int id)
        {
            var deleteCourse = _context.Courses!.SingleOrDefault(b => b.CourseID == id);
            if (deleteCourse != null)
            {
                _context.Courses!.Remove(deleteCourse);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<CourseDTO>> GetAllCoursesAsync()
        {
            var Courses = await _context.Courses!.ToListAsync();
            return _mapper.Map<List<CourseDTO>>(Courses);
        }

        public async Task<CourseDTO> GetCourseAsync(int id)
        {
            var Courses = await _context.Courses!.FindAsync(id);
            return _mapper.Map<CourseDTO>(Courses);
        }

        public async Task UpdateCourseAsync(int id, CourseDTO model)
        {
            if (id == model.CourseID)
            {
                var updateCourse = _mapper.Map<Course>(model);
                _context.Courses!.Update(updateCourse);
                await _context.SaveChangesAsync();
            }
        }
    }
}
