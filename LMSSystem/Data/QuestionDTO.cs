using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Data
{
    public class QuestionDTO
    {
        public int QuestionID { get; set; }
        public int ExamID { get; set; }
        public string? QuestionContent { get; set; } = string.Empty;
        public string? QuestionType { get; set; } = string.Empty;

    }
}
