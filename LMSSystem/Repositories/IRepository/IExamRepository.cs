using LMSSystem.Data;

namespace LMSSystem.Repositories.IRepository
{
    public interface IExamRepository
    {
        public Task<List<ExamDTO>> GetAllExamsAsync();
        public Task<ExamDTO> GetExamAsync(int id);
        public Task<int> AddExamAsync(ExamDTO model);
        public Task UpdateExamAsync(int id, ExamDTO model);
        public Task DeleteExamAsync(int id);
    }
}
