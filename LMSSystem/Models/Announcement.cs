using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Models
{
    public class Announcement
    {
        [Key]
        public int AnnouncementID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        [Required]
        public string? AnnouncementContent { get; set; }

        [Required]
        public DateTime AnnouncementDate { get; set; }

        public virtual User? User { get; set; }
    }
}
