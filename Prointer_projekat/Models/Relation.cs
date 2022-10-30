namespace Prointer_projekat.Models;

public record Relation
{
    public int Id { get; set; }
    public string Value { get; set; } = null!;
    
    public int ArticleId { get; set; }
    public Article Article { get; set; } = null!;

    public int AttributeId { get; set; }
    public Attribute Attribute { get; set; } = null!;
};