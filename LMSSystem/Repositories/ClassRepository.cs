using AutoMapper;
using LMSSystem.Data;
using LMSSystem.Models;
using LMSSystem.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LMSSystem.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly SchoolContext _context;
        private readonly IMapper _mapper;

        public ClassRepository(SchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddClassAsync(ClassDTO model)
        {
            var newClass = _mapper.Map<Class>(model);
            _context.Classes!.Add(newClass);
            await _context.SaveChangesAsync();

            return newClass.ClassID;
        }

        public async Task DeleteClassAsync(int id)
        {
            var deleteClass = _context.Classes!.SingleOrDefault(b => b.ClassID == id);
            if (deleteClass != null)
            {
                _context.Classes!.Remove(deleteClass);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ClassDTO>> GetAllClasssAsync()
        {
            var Classs = await _context.Classes!.ToListAsync();
            return _mapper.Map<List<ClassDTO>>(Classs);
        }

        public async Task<ClassDTO> GetClassAsync(int id)
        {
            var Classs = await _context.Classes!.FindAsync(id);
            return _mapper.Map<ClassDTO>(Classs);
        }

        public async Task UpdateClassAsync(int id, ClassDTO model)
        {
            if (id == model.ClassID)
            {
                var updateClass = _mapper.Map<Class>(model);
                _context.Classes!.Update(updateClass);
                await _context.SaveChangesAsync();
            }
        }
    }
}
