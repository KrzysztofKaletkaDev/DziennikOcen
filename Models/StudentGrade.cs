using System.ComponentModel.DataAnnotations;

namespace DziennikOcen.Models
{
    public class StudentGrade
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public int LecturerId { get; set; }
        public User Lecturer { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public int GradeScaleId { get; set; }
        public GradeScale GradeScale { get; set; } = null!;

        public int AssessmentTypeId { get; set; }
        public AssessmentType AssessmentType { get; set; } = null!;

        [Required]
        public DateTime DateAssigned { get; set; } = DateTime.Now;

        public string? OptionalDescription { get; set; }
    }
}