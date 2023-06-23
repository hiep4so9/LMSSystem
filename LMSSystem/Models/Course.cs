using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Models
{
    public class Course
    {
        [Key]
        public int CourseID { get; set; }

        [Required]
        public string? CourseName { get; set; }
    }
}
