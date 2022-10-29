using System.ComponentModel.DataAnnotations;

namespace Prointer_projekat.DTOs;

//ovaj DTO se koristi tokom registracije novih korisnika
public record NewUser
{
    [Required] 
    [DataType(DataType.EmailAddress)]
    [EmailAddress] 
    public string Email { get; set; }
    [Required]
    public string Name { get; set; } 
    [Required]
    public string Surname { get; set; }
    [Required]
    public string Password { get; set; } 
    [Required]
    public string RepeatedPass { get; set; } 
    [Required]
    [DataType(DataType.PhoneNumber)]
    [Phone]
    public string PhoneNumber { get; set; } 
}
