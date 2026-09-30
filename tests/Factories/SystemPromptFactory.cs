// SystemPromptServiceFactory.cs

using Yggdrasil.Models.Entities;
using Yggdrasil.Models.DTO;
using AutoBogus;
using Yggdrasil.Data;

namespace Yggdrasil.Tests.Factories;

public static class SystemPromptFactory
{
    public static SystemPrompt Create(AppDbContext db){
        var entity = new AutoFaker<SystemPrompt>().RuleFor(x => x.ID, _ => Guid.Empty).Generate();
        db.Set<SystemPrompt>().Add(entity);
        db.SaveChanges();
        return entity;
    }
}
