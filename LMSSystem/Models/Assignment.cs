using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Models
{
    public class Assignment
    {
        [Key]
        public int AssignmentID { get; set; }

        [ForeignKey("Class")]
        public int ClassID { get; set; }

        [Required]
        public string? AssignmentTitle { get; set; }

        public string? AssignmentFile { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        public virtual Class? Class { get; set; }
        public virtual ICollection<Class>? Classes { get; set; }
    }
}
