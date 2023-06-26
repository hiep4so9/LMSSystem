using AutoMapper;
using LMSSystem.Data;
using LMSSystem.Models;
using LMSSystem.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LMSSystem.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly SchoolContext _context;
        private readonly IMapper _mapper;

        public MaterialRepository(SchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddMaterialAsync(MaterialDTO model)
        {
            var newMaterial = _mapper.Map<Material>(model);
            _context.Materials!.Add(newMaterial);
            await _context.SaveChangesAsync();
            return newMaterial.MaterialID;
        }


        public async Task DeleteMaterialAsync(int id)
        {
            var deleteMaterial = _context.Materials!.SingleOrDefault(b => b.MaterialID == id);
            if (deleteMaterial != null)
            {
                _context.Materials!.Remove(deleteMaterial);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<MaterialDTO>> GetAllMaterialsAsync()
        {
            var Materials = await _context.Materials!.ToListAsync();
            return _mapper.Map<List<MaterialDTO>>(Materials);
        }

        public async Task<MaterialDTO> GetMaterialAsync(int id)
        {
            var Materials = await _context.Materials!.FindAsync(id);
            return _mapper.Map<MaterialDTO>(Materials);
        }


        public async Task<List<MaterialDTO>> GetMaterialByCourseAsync(int id)
        {
            var Materials = await _context.Materials
                .Where(m => m.CourseID == id)
                .ToListAsync();

            return _mapper.Map<List<MaterialDTO>>(Materials);
        }


        public async Task UpdateMaterialAsync(int id, MaterialDTO model)
        {
            var updateMaterial = await _context.Materials.FindAsync(id);
            if (updateMaterial == null)
            {
                return;
            }

            _mapper.Map(model, updateMaterial);
            await _context.SaveChangesAsync();
        }

    }
}
