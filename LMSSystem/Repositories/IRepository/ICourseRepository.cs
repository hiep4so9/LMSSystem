using LMSSystem.Data;

namespace LMSSystem.Repositories.IRepository
{
    public interface ICourseRepository
    {
        public Task<List<CourseDTO>> GetAllCoursesAsync();
        public Task<CourseDTO> GetCourseAsync(int id);
        public Task<int> AddCourseAsync(CourseDTO model);
        public Task UpdateCourseAsync(int id, CourseDTO model);
        public Task DeleteCourseAsync(int id);
    }
}
