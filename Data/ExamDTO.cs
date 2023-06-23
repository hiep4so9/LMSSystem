using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Data
{
    public class ExamDTO
    {
        public int ExamID { get; set; }

        public int ClassID { get; set; }

        public string ExamTitle { get; set; } = string.Empty;

        public string ExamType { get; set; } = string.Empty;

        public TimeSpan Duration { get; set; }
    }
}
