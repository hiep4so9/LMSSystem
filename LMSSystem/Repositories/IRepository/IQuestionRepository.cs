using LMSSystem.Data;

namespace LMSSystem.Repositories.IRepository
{
    public interface IQuestionRepository
    {
        public Task<List<QuestionDTO>> GetAllQuestionsAsync();
        public Task<QuestionDTO> GetQuestionAsync(int id);
        public Task<List<QuestionDTO>> GetQuestionByExamAsync(int id);
        public Task<int> AddQuestionAsync(QuestionDTO model);
        public Task UpdateQuestionAsync(int id, QuestionDTO model);
        public Task DeleteQuestionAsync(int id);
    }
}
