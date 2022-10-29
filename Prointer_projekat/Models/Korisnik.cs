using System.ComponentModel.DataAnnotations;

namespace Prointer_projekat.Models
{
    public record Korisnik
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Ime { get; set; } = null!;
        public string Prezime { get; set; } = null!;
        public string Lozinka { get; set; } = null!;
        public string BrojTelefona { get; set; } = null!;
    }
    
}
