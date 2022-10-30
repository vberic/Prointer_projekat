using System.ComponentModel.DataAnnotations;

namespace Prointer_projekat.DTOs;

public record AttributesOut
{
    public int AttributeId { get; set; }
    public string Name { get; set; } = null!;
    public bool IsNumerical { get; set; } 
    public bool IsUnique { get; set; } 
}