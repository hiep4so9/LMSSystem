using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Data
{
    public class FeedbackDTO
    {
        
        public int FeedbackID { get; set; }

        public int UserID { get; set; }

        public string? FeedbackContent { get; set; } = string.Empty;

        public DateTime FeedbackDate { get; set; }
    }
}
