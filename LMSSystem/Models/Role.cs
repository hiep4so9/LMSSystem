using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }

        [Required]
        public string? RoleName { get; set; }
    }
}
