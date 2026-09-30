// SystemPromptDTO.cs

using System.ComponentModel.DataAnnotations;
using Yggdrasil.Models.Entities;

namespace Yggdrasil.Models.DTO;

public class SystemPromptDTO{
    public record Request(
        [Required] string Name,
        [Required, MinLength(1)] List<Prompt> prompts
    );
}
