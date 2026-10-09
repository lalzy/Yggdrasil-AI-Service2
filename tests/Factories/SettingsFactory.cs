// SettingsFactory.cs

using AutoBogus;
using Yggdrasil.Models.Entities;
using Yggdrasil.Data;

namespace Yggdrasil.Tests.Factories;

public static class SettingsFactory{
    public static Settings Create(AppDbContext db, SystemPrompt? prompt = null){
        var entity = new AutoFaker<Settings>().RuleFor(x => x.ID, _ => Guid.Empty)
            .RuleFor(x => x.DefaultPrompt, _ => prompt ??= SystemPromptFactory.Create(db)).Generate();
        
        db.Set<Settings>().Add(entity);
        db.SaveChanges();
        return entity;
    }
}
