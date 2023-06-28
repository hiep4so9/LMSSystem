using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Models
{
    public class Course
    {
        [Key]
        public int CourseID { get; set; }

        [Required]
        public string? CourseName { get; set; }
        public ICollection<Exam>? Exams { get; set; }
        public ICollection<Lesson>? Lessons { get; set; }
        public ICollection<Material>? Materials { get; set; }
        public ICollection<Schedule>? Schedules { get; set; }
    }
}
