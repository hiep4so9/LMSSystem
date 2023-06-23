using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Models
{
    public class Class
    {
        [Key]
        public int ClassID { get; set; }

        [Required]
        public string? ClassName { get; set; }
    }
}
