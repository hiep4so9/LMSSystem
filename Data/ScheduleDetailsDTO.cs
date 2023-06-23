namespace LMSSystem.Data
{
    public class ScheduleDetailsDTO
    {
        public int ScheduleID { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public DateTime ScheduleDate { get; set; }
    }
}
