using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        [Required]
        public string? FeedbackContent { get; set; }

        [Required]
        public DateTime FeedbackDate { get; set; }


        public virtual User? User { get; set; }
    }
}
