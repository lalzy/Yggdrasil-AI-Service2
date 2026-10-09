// SystempromptService.cs

using Yggdrasil.Data;
using Yggdrasil.Models.Entities;
using Yggdrasil.Models.DTO;
using Yggdrasil.Models.Enums;

namespace Yggdrasil.Services;

public class SystemPromptServices(AppDbContext db){
    private readonly DbSet<SystemPrompt> _systemPrompt = db.Set<SystemPrompt>();
    
    /// <summary>Create and store a SystemPrompt to the Database</summary>
    /// <param name="request">A SystemPromptDTO Request</param>
    /// <returns>The saved SystemPrompt</returns>
    public SystemPrompt Create(SystemPromptDTO.Request request){
        var entity = new SystemPrompt { Name = request.Name};
        _systemPrompt.Add(entity);
        db.SaveChanges();
        return entity;
    }
    
    /// <summary>Fetch a requested SystemPrompt</summary>
    /// <param name="ID">The Guid of the systemPrompt</param>
    /// <returns>System Prompt if found, otherwise null</returns>
    public SystemPrompt? Get(Guid ID){
        return _systemPrompt.Include(sp => sp.Prompts.OrderBy(p => p.Order)).FirstOrDefault(sp => sp.ID == ID);
    }

    /// <summary>Fetch all System prompts</summary>
    /// <param name="count">How many to fetch per page</param>
    /// <param name="pageIndex">Page index to fetch from</param>
    /// <param name="sortOrder">Sorting order.</param>
    public List<SystemPrompt> GetAll(int count=10, int pageIndex=0, SortOrder sortOrder=SortOrder.IDAsc){
        var request = _systemPrompt.AsNoTracking();

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

    /// <summary>Renames the SystemPrompt's identifying name</summary>
    /// <param name="ID">The SystemPrompt ID to edit</param>
    /// <param name="name">The new name</param>
    /// <returns>The changed SystemPrompt</returns>
    public SystemPrompt Rename(Guid ID, string name){
        var entity = _systemPrompt.Find(ID)!;
        entity.Name = name;
        db.SaveChanges();
        return entity;
    }

    /// <summary>Delete System prompt</summary>
    /// <param name="ID">The SystemPrompt ID to delete</param>
    public void Delete(Guid ID){
        
    }

    /// <summary>Add a prompt to the systemPrompt</summary>
    /// <param name="ID">SystemPrompt ID to add to</param>
    /// <param name="PromptID">Prompt ID to add</param>
    /// <returns>The adjusted SystemPrompt with the Prompt</returns>
    public SystemPrompt AddPrompt(Guid ID, Guid PromptID){
        return new();
    }
    
    /// <summary>Remove a prompt to the systemPrompt</summary>
    /// <param name="ID">SystemPrompt ID to add to</param>
    /// <param name="PromptID">Prompt ID to add</param>
    /// <returns>The adjusted SystemPrompt with the Prompt</returns>
    public SystemPrompt RemovePrompt(Guid ID, Guid PromptID){
        return new();
    }
}
