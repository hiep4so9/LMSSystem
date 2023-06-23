using LMSSystem.Data;

namespace LMSSystem.Repositories.IRepository
{
    public interface ILessonRepository
    {
        public Task<List<LessonDTO>> GetAllLessonsAsync();
        public Task<LessonDTO> GetLessonAsync(int id);
        public Task<int> AddLessonAsync(LessonDTO model);
        public Task UpdateLessonAsync(int id, LessonDTO model);
        public Task DeleteLessonAsync(int id);
    }
}
