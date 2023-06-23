using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Models
{
    public class Question
    {
        [Key]
        public int QuestionID { get; set; }

        [ForeignKey("Exam")]
        public int ExamID { get; set; }

        [Required]
        public string? QuestionContent { get; set; }

        [Required]
        public string? QuestionType { get; set; }

        public virtual Exam? Exam { get; set; }
    }
}
