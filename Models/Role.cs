using System.ComponentModel.DataAnnotations;

namespace DziennikOcen.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa roli jest wymagana")]
        [StringLength(20)]
        public string Name { get; set; } = string.Empty;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}