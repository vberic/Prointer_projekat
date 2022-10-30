using System.ComponentModel.DataAnnotations;

namespace Prointer_projekat.DTOs;

public record RemoveAttribute
{
    [Required]
    public string ArticleName { get; set; } = null!;
    [Required]
    public string AttributeName { get; set; } = null!;
 
    public string? Value { get; set; }
}