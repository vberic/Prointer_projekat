using System.ComponentModel.DataAnnotations;

namespace Prointer_projekat.DTOs;

public record UpdateAttributes
{
    [Required]
    public string ArticleName { get; set; } = null!;
    [Required]
    public string AttributeName { get; set; } = null!;
    [Required]
    public string OldValue { get; set; } = null!;
    [Required]
    public string NewValue { get; set; } = null!;
}