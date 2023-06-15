using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Data
{
    public class AnnouncementDTO
    {
        [Key]
        public int AnnouncementID { get; set; } 
        public int UserID { get; set; }
        public string? AnnouncementContent { get; set; } = string.Empty;
        public DateTime AnnouncementDate { get; set; }

    }
}
