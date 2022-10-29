using Microsoft.AspNetCore.Mvc;
using Prointer_projekat.Data;
using Prointer_projekat.DTOs;
using Prointer_projekat.Models;

namespace Prointer_projekat.Controllers
{
    [ApiController]
    [Route("korisnici")]
    public class KorisnikController : ControllerBase
    {
        private readonly ConnectionManager _context = new ConnectionManager();
        
        
        [HttpPost]
        public ActionResult Registracija(NoviKorisnik novi)
        {
            //provjera da li se lozinke poklapaju
            if (!novi.Lozinka.Equals(novi.PonovljenaLozinka))
                return BadRequest("Lozinke se ne poklapaju");
            
            //provjera da li je mejl vec u upotrebi
            if (_context.Korisnici.Count(k=> k.Email.Equals(novi.Email)) != 0)
                return BadRequest("Korisnik sa ovim emailom je vec registrovan");

            Korisnik kor = new()
            {
                Email = novi.Email,
                Ime = novi.Ime,
                Prezime = novi.Prezime,
                BrojTelefona = novi.BrojTelefona,
                Lozinka = novi.Lozinka
            };
            _context.Korisnici.Add(kor);
            _context.SaveChanges();

            return Ok("Korisnik sa emailom: "+kor.Email+" uspjesno registrovan");
        }
    }
}