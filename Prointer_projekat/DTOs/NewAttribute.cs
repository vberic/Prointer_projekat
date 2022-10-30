using System.ComponentModel.DataAnnotations;

namespace Prointer_projekat.DTOs;

public record NewAttribute
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public bool IsNumerical { get; set; }
    [Required]
    public bool IsUnique { get; set; }
}