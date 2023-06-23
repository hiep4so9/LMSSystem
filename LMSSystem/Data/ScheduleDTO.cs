using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Data
{
    public class ScheduleDTO
    {
        public int ScheduleID { get; set; }

        public int ClassID { get; set; }

        public int CourseID { get; set; }
        public DateTime ScheduleDate { get; set; }
    }
}
