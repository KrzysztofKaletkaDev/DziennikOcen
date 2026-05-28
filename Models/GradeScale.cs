using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DziennikOcen.Models
{
    public class GradeScale
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(3,1)")]
        public decimal Value { get; set; }

        public ICollection<StudentGrade> Grades { get; set; } = new List<StudentGrade>();
    }
}