using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Models
{
    public class Exam
    {
        [Key]
        public int ExamID { get; set; }

        [ForeignKey("Course")]
        public int CourseID { get; set; }

        [Required]
        public string? ExamTitle { get; set; }

        [Required]
        public string? ExamType { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }
        public virtual Course? Course { get; set; }
        public virtual ICollection<Question>? Questions { get; set; }
        public virtual ICollection<Exam_User>? Exam_User { get; set; }
    }
}
