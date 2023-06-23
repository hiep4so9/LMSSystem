using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Models
{
    public class Message
    {
        [Key]
        public int MessageID { get; set; }

        [ForeignKey("Sender")]
        public int SenderID { get; set; }

        [ForeignKey("Receiver")]
        public int ReceiverID { get; set; }

        [Required]
        public string? MessageContent { get; set; }

        [Required]
        public DateTime MessageDate { get; set; }

        public virtual User? Sender { get; set; }
        public virtual User? Receiver { get; set; }
    }
}
