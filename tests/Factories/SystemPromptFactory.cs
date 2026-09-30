// SystemPromptServiceFactory.cs

using Yggdrasil.Models.Entities;
using Yggdrasil.Models.DTO;
using AutoBogus;
using Yggdrasil.Data;

namespace Yggdrasil.Tests.Factories;

public static class SystemPromptFactory
{
    public static SystemPrompt Create(AppDbContext db, Prompt? prompt=null){
        prompt ??= CreatePrompt(db);
        var entity = new AutoFaker<SystemPrompt>()
            .RuleFor(x => x.ID, _ => Guid.Empty)
            .RuleFor(x => x.Prompts, _ => new List<Prompt>{prompt})
            .Generate();
        db.Set<SystemPrompt>().Add(entity);
        db.SaveChanges();
        return entity;
    }

    public static Prompt CreatePrompt(AppDbContext db){
        var entity = new AutoFaker<Prompt>().RuleFor(x => x.ID, _ => Guid.Empty).Generate();
        db.Set<Prompt>().Add(entity);
        db.SaveChanges();
        return entity;
    }
}
