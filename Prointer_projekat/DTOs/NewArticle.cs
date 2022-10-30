using System.ComponentModel.DataAnnotations;

namespace Prointer_projekat.DTOs;

public record NewArticle
{
    [Required]
    public int ArticleId { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Unit { get; set; } = null!;
    [Required]
    public string Manufacturer { get; set; } = null!;

}