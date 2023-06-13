using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Models
{
    public class User_Schedule
    {

        [Key]
        public int UserID { get; set; }

        [Key]
        public int ScheduleID { get; set; }

        [Required]
        public string? IsPresent { get; set; }

        public virtual User? User { get; set; }
        public virtual Schedule? Schedule { get; set; }
    }
}
