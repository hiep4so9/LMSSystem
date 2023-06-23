using LMSSystem.Data;

namespace LMSSystem.Repositories.IRepository
{
    public interface IAnnouncementRepository
    {
        public Task<List<AnnouncementDTO>> GetAllAnnouncementsAsync();
        public Task<AnnouncementDTO> GetAnnouncementAsync(int id);
        public Task<int> AddAnnouncementAsync(AnnouncementDTO model);
        public Task UpdateAnnouncementAsync(int id, AnnouncementDTO model);
        public Task DeleteAnnouncementAsync(int id);
    }
}
