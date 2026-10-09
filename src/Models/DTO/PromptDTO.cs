// PromptDTO.cs

using System.ComponentModel.DataAnnotations;
using Yggdrasil.Models.Entities;

namespace Yggdrasil.Models.DTO;

public class PromptDTO{
    public record Request(
        [Required] string Name,
        [Required] string Content,
        SourceType Source,
        int Order,
        bool Active
    );
    public record UpdateRequest(
        string? Name = null,
        string? Content = null,
        SourceType? Source = null,
        int? Order = null,
        bool? Active = null
    );
}
