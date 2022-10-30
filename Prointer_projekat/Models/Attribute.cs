using System.ComponentModel.DataAnnotations;

namespace Prointer_projekat.Models;

public record Attribute
{
    [Key]
    public int AttributeId { get; set; }
    public string Name { get; set; } = null!;
    [Required]
    public bool IsNumerical { get; set; } // odredjuje da li atribut treba da ima numericku ili alfanumericku vrijednost
    [Required]
    public bool IsUnique { get; set; } // odredjuje da li artikl moze da ima samo jedan atribut sa tim imeno ili vise njih 

    public ICollection<Relation> Relations { get; set; } = null!;
};