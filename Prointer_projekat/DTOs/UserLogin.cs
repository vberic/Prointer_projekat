using System.ComponentModel.DataAnnotations;

namespace Prointer_projekat.DTOs;

public record UserLogin
{
    [Required]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string Email { get; set; }= null!;
    [Required]
    public string Password { get; set; }= null!;
}