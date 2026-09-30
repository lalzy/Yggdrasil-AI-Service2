// SystemPrompt.cs

namespace Yggdrasil.Models.Entities;

public class SystemPrompt{
    public Guid ID { get; set; }
    public string? Name { get; set; }
    public List<Prompt> Prompts { get; set; } = new();
}
