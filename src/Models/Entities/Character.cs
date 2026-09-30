// Character.cs

namespace Yggdrasil.Models;

public class Character  : CharacterBase {
    /// <summary>What role it holds in the current conversation. Such as; "friend of user"</summary>
    public string? NarrativeRole {get; set;}
    public required string Personality {get; set;}
    /// <summary>Example dialogues for how the character should act.</summary>
    public List<string>? ExampleDialogue { get; set; } = [];
    /// <summary>Conversation this character is owned to.</summary>
    /// <remarks>This is for instanced characters, master template (of character) is null on conversation ID.</remarks>
    public Guid? Conversation_ID { get; set; }
}
