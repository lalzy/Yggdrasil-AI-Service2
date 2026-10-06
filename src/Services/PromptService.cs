// PromptService.cs

using Yggdrasil.Data;
using Yggdrasil.Models.Entities;
using Yggdrasil.Models.DTO;
using Yggdrasil.Models.Enums;

namespace Yggdrasil.Services;

public class PromptServices(AppDbContext db) {
    /// <summary></summary>
    /// <param name="request">The PromptDTO Request</param>
    /// <returns></returns>
    public Prompt Create(PromptDTO.Request request){
        return new();
    }

    /// <summary></summary>
    /// <param name="ID">The Prompt ID</param>
    /// <returns></returns>
    public Prompt Get(Guid ID){
        return new();
    }

    /// <summary></summary>
    /// <param name="ID">The Prompt ID</param>
    /// <param name="request">The PromptDTO updateRequest</param>
    /// <returns></returns>
    public Prompt Update(Guid ID, PromptDTO.UpdateRequest request){
        return new();
    }

    /// <summary>Delete the Prompt</summary>
    public void Delete(){
        
    }
}
