using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Data
{
    public class RoleDTO
    {
        public int RoleID { get; set; }
        public string? RoleName { get; set; } = string.Empty;
    }
}
