using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Data
{
    public class MessageDTO
    {
        public int MessageID { get; set; }

        public int SenderID { get; set; }

        public int ReceiverID { get; set; }

        public string? MessageContent { get; set; } = string.Empty;

        public DateTime MessageDate { get; set; }

    }
}
