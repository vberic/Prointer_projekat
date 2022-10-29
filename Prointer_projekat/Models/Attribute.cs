using System.ComponentModel.DataAnnotations;

namespace Prointer_projekat.Models;

public record Attribute
{
    [Key]
    public int AttributeId { get; set; }
    public string Name { get; set; } = null!;
    [Required]
    public bool IsNumerical { get; set; }
    [Required]
    public bool IsUnique { get; set; }

    public ICollection<Relation> Relations { get; set; }
};