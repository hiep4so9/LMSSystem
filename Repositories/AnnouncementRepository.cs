using AutoMapper;
using LMSSystem.Data;
using LMSSystem.Models;
using LMSSystem.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LMSSystem.Repositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly SchoolContext _context;
        private readonly IMapper _mapper;

        public AnnouncementRepository(SchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddAnnouncementAsync(AnnouncementDTO model)
        {
            var newAnnouncement = _mapper.Map<Announcement>(model);
            _context.Announcements!.Add(newAnnouncement);
            await _context.SaveChangesAsync();

            return newAnnouncement.AnnouncementID;
        }

        public async Task DeleteAnnouncementAsync(int id)
        {
            var deleteAnnouncement = _context.Announcements!.SingleOrDefault(b => b.AnnouncementID == id);
            if (deleteAnnouncement != null)
            {
                _context.Announcements!.Remove(deleteAnnouncement);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<AnnouncementDTO>> GetAllAnnouncementsAsync()
        {
            var Announcements = await _context.Announcements!.ToListAsync();
            return _mapper.Map<List<AnnouncementDTO>>(Announcements);
        }

        public async Task<AnnouncementDTO> GetAnnouncementAsync(int id)
        {
            var Announcements = await _context.Announcements!.FindAsync(id);
            return _mapper.Map<AnnouncementDTO>(Announcements);
        }

        public async Task UpdateAnnouncementAsync(int id, AnnouncementDTO model)
        {
            if (id == model.AnnouncementID)
            {
                var updateAnnouncement = _mapper.Map<Announcement>(model);
                _context.Announcements!.Update(updateAnnouncement);
                await _context.SaveChangesAsync();
            }
        }
    }
}
