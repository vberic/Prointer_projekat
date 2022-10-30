using System.ComponentModel.DataAnnotations;

namespace Prointer_projekat.DTOs;

public record UpdateArticle
{
    [Required]
    public int ArticleId { get; set; }
    public string? Name { get; set; } 
    public string? Unit { get; set; } 
    public string? Manufacturer { get; set; }
}