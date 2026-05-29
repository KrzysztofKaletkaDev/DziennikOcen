using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DziennikOcen.Models
{
    public class User
    {
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

        [Required(ErrorMessage = "Adres email jest wymagany")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public bool IsActive { get; set; } = true;

        public ICollection<StudentGrade> GivenGrades { get; set; } = new List<StudentGrade>();
    }
}
