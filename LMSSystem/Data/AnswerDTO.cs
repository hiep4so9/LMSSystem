using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Data
{
    public class AnswerDTO
    {
        public int AnswerID { get; set; }

        public int QuestionID { get; set; }

        public string AnswerContent { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
        
    }
}
