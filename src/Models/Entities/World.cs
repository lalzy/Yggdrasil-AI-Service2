/// World.cs

namespace Yggdrasil.Models.Entities;

public class World{
    public Guid ID {get; set;}
    /// <summary>ID of attached lorebook.</summary>
    public Guid? Lorebook_ID {get; set;}
    /// <summary>World name, not given to LLM.</summary>
    public required string Name {get; set;}
    /// <summary>Description of the world. User faced, does not get sent to LLM</summary>
    public required string Description {get; set;}
    /// <summary>Intro message that sets up the scenario. Optional</summary>
    public string? IntroMessage { get; set; }
    /// <summary>Instructions to the narrator LLM. Things like style, do's and don'ts that you want this specific world to adher to</summary>
    public string? NarratorInstruction {get; set;}
    /// <summary>Current scenario over-view, sent to the LLM.</summary>
    public string? Scenario { get; set; }
    /// <summary>All characters in the world</summary>
    public List<Character> Characters {get; set;} = [];
    /// <summary>Example dialogue of how you want the LLM to respond</summary>
    public List<string> NarratorExampleDialogue {get;set;} = new();
    public DateTime LastUsed { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public DateTime UpdatedAt {get; set;} = DateTime.UtcNow;
}
