// PromptFactory.cs

using Yggdrasil.Models.Entities;
using Yggdrasil.Models.DTO;
using AutoBogus;
using Yggdrasil.Data;

namespace Yggdrasil.Tests.Factories;

public static class PromptFactory
{
    public static Prompt CreatePrompt(AppDbContext db)
    {
        var entity = new AutoFaker<Prompt>().RuleFor(x => x.ID, _ => Guid.Empty).Generate();
        db.Set<Prompt>().Add(entity);
        db.SaveChanges();
        return entity;
    }
}
