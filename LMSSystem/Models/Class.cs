using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Models
{
    public class Class
    {
        [Key]
        public int ClassID { get; set; }

        [Required]
        public string? ClassName { get; set; }
        public ICollection<Schedule>? Schedules { get; set; }
        public ICollection<Assignment>? Assignments { get; set; }
        public ICollection<User_Class>? User_Class { get; set; }
    }
}
