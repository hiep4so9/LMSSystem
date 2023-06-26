namespace LMSSystem.Repositories.IRepository
{
    public interface IMeetingRepository
    {
        Task<string> GetMeetingDetails(int scheduleId);
    }
}
