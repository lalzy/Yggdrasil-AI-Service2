// SystempromptService.cs

using Yggdrasil.Data;
using Yggdrasil.Models.Entities;
using Yggdrasil.Models.DTO;
using Yggdrasil.Models.Enums;

namespace Yggdrasil.Services;

public class SystemPromptServices(AppDbContext db){
    /// <summary>Create and store a SystemPrompt to the Database</summary>
    /// <param name="request">A SystemPromptDTO Request</param>
    /// <returns>The saved SystemPrompt</returns>
    public SystemPrompt Create(SystemPromptDTO.Request request){
        var entity = new SystemPrompt { Name = request.Name, Prompts = request.prompts};
        db.Set<SystemPrompt>().Add(entity);
        db.SaveChanges();
        return entity;
    }

    /// <summary>Fetch a requested SystemPrompt</summary>
    /// <param name="ID">The Guid of the systemPrompt</param>
    /// <returns>System Prompt if found, otherwise null</returns>
    public SystemPrompt? Get(Guid ID){
        return db.Set<SystemPrompt>().Include(sp => sp.Prompts).FirstOrDefault(sp => sp.ID == ID);
    }

    /// <summary>Fetch all System prompts</summary>
    /// <param name="count">How many to fetch per page</param>
    /// <param name="pageIndex">Page index to fetch from</param>
    /// <param name="sortOrder">Sorting order.</param>
    public List<SystemPrompt> GetAll(int count=10, int pageIndex=0, SortOrder sortOrder=SortOrder.IDAsc){
        var request = db.Set<SystemPrompt>().AsNoTracking();

        request = sortOrder switch{
            SortOrder.IDAsc => request.OrderBy(sp => sp.ID),
            SortOrder.IDDesc => request.OrderByDescending(sp => sp.ID),
            SortOrder.NameAsc => request.OrderBy(sp => EF.Functions.Collate(sp.Name, "NOCASE")).ThenBy(sp => sp.ID),
            SortOrder.NameDesc => request.OrderByDescending(sp => EF.Functions.Collate(sp.Name, "NOCASE")).ThenBy(sp => sp.ID),
            SortOrder.CreatedAsc => request.OrderBy(sp => sp.CreatedAt).ThenBy(sp => sp.ID),
            SortOrder.CreatedDesc => request.OrderByDescending(sp => sp.CreatedAt).ThenBy(sp => sp.ID),
            SortOrder.ModifiedAsc => request.OrderBy(sp => sp.UpdatedAt).ThenBy(sp => sp.ID),
            SortOrder.ModifiedDesc => request.OrderByDescending(sp => sp.UpdatedAt).ThenBy(sp => sp.ID),
            _ => throw new ArgumentOutOfRangeException(nameof(sortOrder), sortOrder, null)
        };
        return request.Include(sp => sp.Prompts).Skip(pageIndex * count).Take(count).ToList();
    }

    /// <summary></summary>
    /// <param name="ID">The SystemPrompt ID to edit</param>
    /// <param name="request">The updatedDTO Request</param>
    /// <returns>The changed SystemPrompt</returns>
    public SystemPrompt Update(Guid ID, SystemPromptDTO.UpdateRequest request){
        return new SystemPrompt();
    }

    /// <summary>Delete System prompt</summary>
    /// <param name="ID">The SystemPrompt ID to delete</param>
    public void Delete(Guid ID){
        
    }
}
