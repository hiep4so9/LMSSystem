using LMSSystem.Data;

namespace LMSSystem.Repositories.IRepository
{
    public interface IClassRepository
    {
        public Task<List<ClassDTO>> GetAllClassesAsync();
        public Task<ClassDTO> GetClassAsync(int id);
        public Task<int> AddClassAsync(ClassDTO model);
        public Task UpdateClassAsync(int id, ClassDTO model);
        public Task DeleteClassAsync(int id);
    }
}
