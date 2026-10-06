// PromptDTO.cs

using System.ComponentModel.DataAnnotations;
using Yggdrasil.Models.Entities;

namespace Yggdrasil.Models.DTO;

public class PromptDTO{
    public record Request(
        [Required] string Name,
        [Required] string Content,
        SourceType Source,
        int order,
        bool active
    );
    public record UpdateRequest(
        string? Name,
        string? Content,
        SourceType? Source,
        int? order,
        bool? active
    );
}
