using LMSSystem.Data;

namespace LMSSystem.Repositories.IRepository
{
    public interface IScheduleRepository
    {
        public Task<List<ScheduleDTO>> GetAllSchedulesAsync();
        public Task<ScheduleDTO> GetScheduleAsync(int id);
        public Task<int> AddScheduleAsync(ScheduleDTO model);
        public Task<List<ScheduleDetailsDTO>> GetAllSchedulesDetailAsync();
        public Task UpdateScheduleAsync(int id, ScheduleDTO model);
        public Task DeleteScheduleAsync(int id);
    }
}
