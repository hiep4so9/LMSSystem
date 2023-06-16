using LMSSystem.Data;

namespace LMSSystem.Repositories.IRepository
{
    public interface IAnswerRepository
    {
        public Task<List<AnswerDTO>> GetAllAnswersAsync();
        public Task<AnswerDTO> GetAnswerAsync(int id);
        public Task<int> AddAnswerAsync(AnswerDTO model);
        public Task UpdateAnswerAsync(int id, AnswerDTO model);
        public Task DeleteAnswerAsync(int id);
    }
}
