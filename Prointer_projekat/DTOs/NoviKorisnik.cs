using System.ComponentModel.DataAnnotations;

namespace Prointer_projekat.DTOs
{
    //ovaj DTO se koristi tokom registracije novih korisnika
    public record NoviKorisnik
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Ime { get; set; } 
        [Required]
        public string Prezime { get; set; }
        [Required]
        public string Lozinka { get; set; } 
        [Required]
        public string PonovljenaLozinka { get; set; } 
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string BrojTelefona { get; set; } 
    }
}