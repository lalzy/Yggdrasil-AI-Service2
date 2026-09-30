// CharacterBase.cs

namespace Yggdrasil.Models;

public abstract class CharacterBase{
    public Guid ID {get; set;}
    /// <summary>Both visual and sent to LLM as xml-tag identifier</summary>
    public required string Name {get; set;}
    public required string Gender {get; set;}
    /// <summary>Description about the character. Sent to LLM.</summary>
    public required string Description { get; set; }
    /// <summary>Combat pronoun confusion of the LLM</summary>
    public string? Pronouns { get; set; }
    public string? FullName { get; set; }
    public string? Race {get; set;}
    public string? Occupation {get; set;}
    public string? Appearance {get; set;}
    public string? Equipment {get; set;}
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public DateTime UpdatedAt {get; set;} = DateTime.UtcNow;
}
