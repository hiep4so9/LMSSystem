using AutoMapper;
using LMSSystem.Data;
using LMSSystem.Models;
using LMSSystem.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LMSSystem.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly SchoolContext _context;
        private readonly IMapper _mapper;

        public AssignmentRepository(SchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddAssignmentAsync(AssignmentDTO model)
        {
            var newAssignment = _mapper.Map<Assignment>(model);
            _context.Assignments!.Add(newAssignment);
            await _context.SaveChangesAsync();

            return newAssignment.AssignmentID;
        }

        public async Task DeleteAssignmentAsync(int id)
        {
            var deleteAssignment = _context.Assignments!.SingleOrDefault(b => b.AssignmentID == id);
            if (deleteAssignment != null)
            {
                _context.Assignments!.Remove(deleteAssignment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<AssignmentDTO>> GetAllAssignmentsAsync()
        {
            var Assignments = await _context.Assignments!.ToListAsync();
            return _mapper.Map<List<AssignmentDTO>>(Assignments);
        }

        public async Task<AssignmentDTO> GetAssignmentAsync(int id)
        {
            var Assignments = await _context.Assignments!.FindAsync(id);
            return _mapper.Map<AssignmentDTO>(Assignments);
        }

        public async Task UpdateAssignmentAsync(int id, AssignmentDTO model)
        {
            if (id == model.AssignmentID)
            {
                var updateAssignment = _mapper.Map<Assignment>(model);
                _context.Assignments!.Update(updateAssignment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
