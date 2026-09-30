/// Settings.cs

namespace Yggdrasil.Models.Entities;

public enum Themes{
    dark=0,
    light=1,
}

public class Settings{
    public Guid ID {get; set;} = Guid.Empty;
    public Themes Theme {get;set;} = Themes.dark;
    /// <summary>Default system Prompt, static fallback and first available</summary>
    public SystemPrompt DefaultPrompt { get; set; }
    public SystemPrompt ActivePrompt { get; set; }
}
