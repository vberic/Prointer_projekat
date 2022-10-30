using System.ComponentModel.DataAnnotations;

namespace Prointer_projekat.DTOs;

public record AttributeValue
{
    [Required]
    public string AttributeName { get; set; } = null!;
    [Required]
    public string Value { get; set; } = null!;
}