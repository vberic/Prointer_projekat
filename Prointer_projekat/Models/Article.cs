using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prointer_projekat.Models;

public record Article
{
    [Required]
    [Key][DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int ArticleId { get; set; }

    public string Name { get; set; } = null!;
    public string Unit { get; set; } = null!;
    public string Manufacturer { get; set; } = null!;
    
    public ICollection<Relation> Relations { get; set; }
}