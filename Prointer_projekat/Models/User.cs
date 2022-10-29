using System.ComponentModel.DataAnnotations;

namespace Prointer_projekat.Models
{
    public record User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }
    
}
