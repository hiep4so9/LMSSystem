using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Models
{
    public class Answer
    {
        [Key]
        public int AnswerID { get; set; }

        [ForeignKey("Question")]
        public int QuestionID { get; set; }

        [Required]
        public string? AnswerContent { get; set; }

        [Required]
        public bool IsCorrect { get; set; }

        public virtual Question? Question { get; set; }
    }
}
