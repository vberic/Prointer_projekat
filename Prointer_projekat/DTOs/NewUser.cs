using System.ComponentModel.DataAnnotations;

namespace Prointer_projekat.DTOs;

//ovaj DTO se koristi tokom registracije novih korisnika
public record NewUser
{
    [Required] 
    [DataType(DataType.EmailAddress)]
    [EmailAddress] 
    public string Email { get; set; }= null!;
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Surname { get; set; }= null!;
    [Required]
    public string Password { get; set; } = null!;
    [Required]
    public string RepeatedPass { get; set; } = null!;
    [Required]
    [DataType(DataType.PhoneNumber)]
    [Phone]
    public string PhoneNumber { get; set; } = null!;
}
