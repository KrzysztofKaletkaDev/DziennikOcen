using System.ComponentModel.DataAnnotations;

namespace DziennikOcen.Models
{
    public class AssessmentType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa klasyfikacji jest wymagana")]
        public string Name { get; set; } = string.Empty;

        public ICollection<StudentGrade> Grades { get; set; } = new List<StudentGrade>();
    }
}