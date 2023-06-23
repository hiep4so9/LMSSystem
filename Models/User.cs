using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace LMSSystem.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [ForeignKey("Role")]
        public int RoleID { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        public string? Address { get; set; }

        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        public string? Phone { get; set; }

        public bool? IsActive { get; set; }

        public string? UserImage { get; set; }

        public string? PasswordResetToken { get; set; }

        public DateTime? ResetTokenExpries { get; set; }

        public string? VerificationToken { get; set; }

        public DateTime? VerifyAt { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenCreated { get; set; }

        public DateTime? RefreshTokenExpries { get; set; }

        public DateTime? CreateAt { get; set; }

        public DateTime? UpdateAt { get; set; }

        public virtual Role? Role { get; set; }
    }
}
