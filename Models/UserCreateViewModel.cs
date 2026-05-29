using System.ComponentModel.DataAnnotations;


namespace DziennikOcen.Models
{
    public class UserCreateViewModel
    {
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

        [Required(ErrorMessage = "Hasło startowe jest wymagane")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Hasło musi mieć co najmniej 6 znaków")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Wybór roli jest wymagany")]
        public int RoleId { get; set; }
    }
}
