using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Data
{
    public class MaterialDTO
    {
        public int MaterialID { get; set; }
        public int CourseID { get; set; }
        public string? MaterialTitle { get; set; } = string.Empty;
        public string? MaterialFile { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
    }
}
