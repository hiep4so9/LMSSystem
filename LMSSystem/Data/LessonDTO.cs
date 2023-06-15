using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Data
{
    public class LessonDTO
    {
        public int LessonID { get; set; }

        public int CourseID { get; set; }

        public string? LessonTitle { get; set; } = string.Empty;

        public string? LessonContent { get; set; } = string.Empty;

        public string? ApprovalStatus { get; set; } = string.Empty;

    }
}
