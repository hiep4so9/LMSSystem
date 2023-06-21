using AutoMapper;
using LMSSystem.Data;
using LMSSystem.Models;
using LMSSystem.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LMSSystem.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly SchoolContext _context;
        private readonly IMapper _mapper;

        public RoleRepository(SchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddRoleAsync(RoleDTO model)
        {
            var newRole = _mapper.Map<Role>(model);
            _context.Roles!.Add(newRole);
            await _context.SaveChangesAsync();

            return newRole.RoleID;
        }

        public async Task DeleteRoleAsync(int id)
        {
            var deleteRole = _context.Roles!.SingleOrDefault(b => b.RoleID == id);
            if (deleteRole != null)
            {
                _context.Roles!.Remove(deleteRole);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<RoleDTO>> GetAllRolesAsync()
        {
            var Roles = await _context.Roles!.ToListAsync();
            return _mapper.Map<List<RoleDTO>>(Roles);
        }

        public async Task<RoleDTO> GetRoleAsync(int id)
        {
            var Roles = await _context.Roles!.FindAsync(id);
            return _mapper.Map<RoleDTO>(Roles);
        }

        public async Task<string> GetRoleNameFromRoleId(int id)
        {
            var role = await _context.Roles.SingleOrDefaultAsync(r => r.RoleID == id);
            return role?.RoleName;
        }

        public async Task UpdateRoleAsync(int id, RoleDTO model)
        {
            if (id == model.RoleID)
            {
                var updateRole = _mapper.Map<Role>(model);
                _context.Roles!.Update(updateRole);
                await _context.SaveChangesAsync();
            }
        }
    }
}
