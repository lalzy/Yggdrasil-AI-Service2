// PromptDTO.cs

namespace Yggdrasil.Models.Enums;

public class PromptDTO{
    public record Edit(
        string? Name = null,
        string? Content = null,
        SourceType? source = null,
        bool? Active = null
    );
}
