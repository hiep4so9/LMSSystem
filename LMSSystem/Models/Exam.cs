using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Models
{
    public class Exam
    {
        [Key]
        public int ExamID { get; set; }

        [ForeignKey("Class")]
        public int ClassID { get; set; }

        [Required]
        public string? ExamTitle { get; set; }

        [Required]
        public string? ExamType { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }
        public virtual Class? Class { get; set; }
    }
}
