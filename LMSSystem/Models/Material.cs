using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Models
{
    public class Material
    {
        [Key]
        public int MaterialID { get; set; }

        [ForeignKey("Course")]
        public int CourseID { get; set; }

        [Required]
        public string? MaterialTitle { get; set; }

        public string? MaterialFile { get; set; }

        [Required]
        public DateTime UploadDate { get; set; } = DateTime.Now;


        public virtual Course? Course { get; set; }
    }
}
