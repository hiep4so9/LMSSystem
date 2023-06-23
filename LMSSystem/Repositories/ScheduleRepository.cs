using AutoMapper;
using LMSSystem.Data;
using LMSSystem.Models;
using LMSSystem.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LMSSystem.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly SchoolContext _context;
        private readonly IMapper _mapper;

        public ScheduleRepository(SchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddScheduleAsync(ScheduleDTO model)
        {

            var newSchedule = _mapper.Map<Schedule>(model);
            _context.Schedules!.Add(newSchedule);
            await _context.SaveChangesAsync();

            return newSchedule.ScheduleID;
        }

        public async Task DeleteScheduleAsync(int id)
        {
            var deleteSchedule = _context.Schedules!.SingleOrDefault(b => b.ScheduleID == id);
            if (deleteSchedule != null)
            {
                _context.Schedules!.Remove(deleteSchedule);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ScheduleDTO>> GetAllSchedulesAsync()
        {
            var Schedules = await _context.Schedules!.ToListAsync();
            return _mapper.Map<List<ScheduleDTO>>(Schedules);
        }

        public async Task<List<ScheduleDetailsDTO>> GetAllSchedulesDetailAsync()
        {
            var schedules = await _context.Schedules!.ToListAsync();

            var scheduleDetailsDTOs = new List<ScheduleDetailsDTO>();
            foreach (var schedule in schedules)
            {
                var scheduleDetailsDTO = new ScheduleDetailsDTO
                {
                    ScheduleID = schedule.ScheduleID,
                    ScheduleDate = schedule.ScheduleDate,
                    ClassName = await GetClassNameFromId(schedule.ClassID),
                    CourseName = await GetCourseNameFromId(schedule.CourseID)
                };

                scheduleDetailsDTOs.Add(scheduleDetailsDTO);
            }

            return scheduleDetailsDTOs;
        }

        private async Task<string> GetClassNameFromId(int classId)
        {
            var classEntity = await _context.Classes.FindAsync(classId);
            return classEntity?.ClassName;
        }

        private async Task<string> GetCourseNameFromId(int courseId)
        {
            var courseEntity = await _context.Courses.FindAsync(courseId);
            return courseEntity?.CourseName;
        }

        public async Task<ScheduleDTO> GetScheduleAsync(int id)
        {
            var Schedules = await _context.Schedules!.FindAsync(id);
            return _mapper.Map<ScheduleDTO>(Schedules);
        }

        public async Task UpdateScheduleAsync(int id, ScheduleDTO model)
        {
            if (id == model.ScheduleID)
            {
                var updateSchedule = _mapper.Map<Schedule>(model);
                _context.Schedules!.Update(updateSchedule);
                await _context.SaveChangesAsync();
            }
        }
    }
}
