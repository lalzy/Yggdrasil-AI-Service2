// SystempromptService.cs

using Yggdrasil.Data;
using Yggdrasil.Models.Entities;
using Yggdrasil.Models.DTO;
using Yggdrasil.Models.Enums;

namespace Yggdrasil.Services;

public class SystemPromptServices(AppDbContext db){
    /// <summary>Crate and store a SystemPrompt to the Database</summary>
    /// <param name="request">A SystemPromptDTO Request</param>
    /// <returns>The saved SystemPrompt</returns>
    public SystemPrompt Create(SystemPromptDTO.Request request){
        var entity = new SystemPrompt { Name = request.Name, Prompts = request.prompts};
        db.Set<SystemPrompt>().Add(entity);
        db.SaveChanges();
        return entity;
    }

    public SystemPrompt Get(Guid SystemPrompt_ID){
        return new SystemPrompt();
    }

    public List<SystemPrompt> GetAll(SortOrder sortOrder=SortOrder.IdentifierAsc, int Count=10){
        return [new SystemPrompt()];
    }
}
