using LMSSystem.Data;

namespace LMSSystem.Repositories.IRepository
{
    public interface IAssignmentRepository
    {
        public Task<List<AssignmentDTO>> GetAllAssignmentsAsync();
        public Task<AssignmentDTO> GetAssignmentAsync(int id);
        public Task<int> AddAssignmentAsync(AssignmentDTO model);
        public Task UpdateAssignmentAsync(int id, AssignmentDTO model);
        public Task DeleteAssignmentAsync(int id);
    }
}
