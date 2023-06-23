using LMSSystem.Data;

namespace LMSSystem.Repositories.IRepository
{
    public interface IRoleRepository
    {
        public Task<List<RoleDTO>> GetAllRolesAsync();
        public Task<RoleDTO> GetRoleAsync(int id);
        public Task<int> AddRoleAsync(RoleDTO model);
        public Task<RoleDTO> GetRoleNameFromRoleId(int id);
        public Task UpdateRoleAsync(int id, RoleDTO model);
        public Task DeleteRoleAsync(int id);
    }
}
