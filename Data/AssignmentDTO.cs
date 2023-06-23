using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Data
{
    public class AssignmentDTO
    {
        public int AssignmentID { get; set; }

        public int ClassID { get; set; }

        public string AssignmentTitle { get; set; } = string.Empty;

        public string AssignmentFile { get; set; } = string.Empty;

        public DateTime Deadline { get; set; }
    }
}
