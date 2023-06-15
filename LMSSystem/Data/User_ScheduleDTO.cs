using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Data
{
    public class User_ScheduleDTO
    {
        public int UserID { get; set; }
        public int ScheduleID { get; set; }
        public string? IsPresent { get; set; } = string.Empty;
    }
}
