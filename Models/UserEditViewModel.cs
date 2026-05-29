using System.ComponentModel.DataAnnotations;

namespace DziennikOcen.Models
{
    public class UserEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane")]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-ZęćłńóśźżĄĆĘŁŃÓŚŹŻ\s]+$", ErrorMessage = "Imię może zawierać tylko litery")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-ZęćłńóśźżĄĆĘŁŃÓŚŹŻ\s-]+$", ErrorMessage = "Nazwisko może zawierać tylko litery i myślnik")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adres e-mail jest wymagany")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu e-mail")]
        public string Email { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Nowe hasło musi mieć co najmniej 6 znaków")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Wybór roli jest wymagany")]
        public int RoleId { get; set; }

        public bool IsActive { get; set; }
    }
}