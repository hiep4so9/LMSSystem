using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMSSystem.Data
{
    public class ClassDTO
    {
        public int ClassID { get; set; }

        public string? ClassName { get; set; } = string.Empty;
    }
}
