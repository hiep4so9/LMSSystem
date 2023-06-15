using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Data
{
    public class User_ClassDTO
    {
        public int UserID { get; set; }

        public int ClassID { get; set; }

        public DateTime EnrollmentDate { get; set; }

    }
}
