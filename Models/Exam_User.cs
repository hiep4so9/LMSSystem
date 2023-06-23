using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Models
{
    public class Exam_User
    {
        [Key]
        public int UserID { get; set; }

        [Key]
        public int ExamID { get; set; }
        public double Score { get; set; }

        [Required]
        public DateTime ExamDate { get; set; }

        public DateTime GradedDate { get; set; }

        public string? Remark { get; set; }

        public virtual User? User { get; set; }
        public virtual Exam? Exam { get; set; }
    }
}
