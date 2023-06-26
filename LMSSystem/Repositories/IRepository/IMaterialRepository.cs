using LMSSystem.Data;

namespace LMSSystem.Repositories.IRepository
{
    public interface IMaterialRepository
    {
        public Task<List<MaterialDTO>> GetAllMaterialsAsync();
        public Task<MaterialDTO> GetMaterialAsync(int id);
        public Task<List<MaterialDTO>> GetMaterialByCourseAsync(int id);
        public Task<int> AddMaterialAsync(MaterialDTO model);
        public Task UpdateMaterialAsync(int id, MaterialDTO model);
        public Task DeleteMaterialAsync(int id);
    }
}
