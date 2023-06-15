using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Data
{
    public class CourseDTO
    {
        public int CourseID { get; set; }

        public string? CourseName { get; set; } = string.Empty;
    }
}
