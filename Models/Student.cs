using System.ComponentModel.DataAnnotations;

namespace DziennikOcen.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane")]
        [StringLength(50, ErrorMessage = "Imię nie może przekraczać 50 znaków")]
        [RegularExpression(@"^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ\s]+$",
            ErrorMessage = "Imię może składać się wyłącznie z liter i spacji")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [StringLength(50, ErrorMessage = "Nazwisko nie może przekraczać 50 znaków")]
        [RegularExpression(@"^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ\s-]+$",
            ErrorMessage = "Nazwisko może zawierać wyłącznie litery, spacje oraz myślnik")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Numer indeksu jest wymagany")]
        [RegularExpression(@"^s\d{5}$",
            ErrorMessage = "Numer indeksu musi zaczynać się od małej litery 's' i zawierać dokładnie 5 cyfr (np. s12345)")]
        public string StudentNumber { get; set; } = string.Empty;

        public ICollection<StudentGrade> Grades { get; set; } = new List<StudentGrade>();
    }
}
