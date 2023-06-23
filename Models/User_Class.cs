using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Models
{
    public class User_Class
    {

        [Key]
        public int UserID { get; set; }

        [Key]
        public int ClassID { get; set; }

        [Required]
        public DateTime EnrollmentDate { get; set; }

        public virtual User? User { get; set; }
        public virtual Class? Class { get; set; }
    }
}
