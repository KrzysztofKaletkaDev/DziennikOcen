using System.ComponentModel.DataAnnotations;

namespace DziennikOcen.Models
{
    public class GradeClassification
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa klasyfikacji jest wymagana.")]
        [StringLength(100, ErrorMessage = "Nazwa kategorii nie może przekraczać 100 znaków.")]
        [Display(Name = "Nazwa kategorii")]
        public string Name { get; set; } = string.Empty;
    }
}