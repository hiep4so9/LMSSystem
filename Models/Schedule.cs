using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleID { get; set; }

        [ForeignKey("Class")]
        public int ClassID { get; set; }

        [ForeignKey("Course")]
        public int CourseID { get; set; }

        [Required]
        public DateTime ScheduleDate { get; set; }

        public virtual Class? Class { get; set; }
        public virtual Course? Course { get; set; }
    }
}
