namespace Prointer_projekat.DTOs;

public record ArticleOut
{
    public int ArticleId { get; set; }
    public string Name { get; set; } = null!;
    public string Unit { get; set; } = null!;
    public string Manufacturer { get; set; } = null!;
    public List<KeyValuePair<string, string>> Attributes { get; set; } 

}