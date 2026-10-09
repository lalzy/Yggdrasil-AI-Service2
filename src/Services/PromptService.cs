// PromptService.cs

using Mapster;
using Yggdrasil.Data;
using Yggdrasil.Models.Entities;
using Yggdrasil.Models.DTO;
using Yggdrasil.Models.Enums;
using Yggdrasil.Extensions;

namespace Yggdrasil.Services;

public class PromptServices(AppDbContext db) {
    private readonly DbSet<Prompt> _prompts = db.Set<Prompt>();
    
    /// <summary>Create a new prompt</summary>
    /// <param name="request">The PromptDTO Request</param>
    /// <returns>The created prompt entry</returns>
    public Prompt Create(PromptDTO.Request request){
        Prompt prompt = request.Adapt<Prompt>();
        _prompts.Add(prompt);
        db.SaveChanges();
        return prompt;
    }

    /// <summary>Get a requested prompt</summary>
    /// <param name="ID">The Prompt ID</param>
    /// <returns>The requested Prompt</returns>
    public Prompt Get(Guid ID){
        return _prompts.Find(ID)!;
    }

    /// <summary>Update a prompts fields</summary>
    /// <param name="ID">The Prompt ID to be updated</param>
    /// <param name="request">The PromptDTO updateRequest</param>
    /// <returns>The updated Entry</returns>
    public Prompt Update(Guid ID, PromptDTO.UpdateRequest request){
        var entity = _prompts.Find(ID)!;
        request.Patch(entity);
        db.SaveChanges();
        return entity;
    }

    /// <summary>Delete the Prompt</summary>
    /// <param name="ID">The Prompt ID to be deleted</param>
    public void Delete(Guid ID){
        var entity = _prompts.Find(ID);
        if(entity == null) return;
        
        _prompts.Remove(entity);
        db.SaveChanges();
    }
}
