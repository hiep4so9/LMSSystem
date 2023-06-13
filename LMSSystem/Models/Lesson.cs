using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Models
{
    public class Lesson
    {
        [Key]
        public int LessonID { get; set; }

        [ForeignKey("Course")]
        public int CourseID { get; set; }

        [Required]
        public string? LessonTitle { get; set; }

        [Required]
        public string? LessonContent { get; set; }

        [Required]
        public string? ApprovalStatus { get; set; }

        public virtual Course? Course { get; set; }
    }
}
