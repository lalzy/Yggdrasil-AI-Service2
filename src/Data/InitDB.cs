// InitDb.cs

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Yggdrasil.Models.Entities;
using Yggdrasil.Services;

namespace Yggdrasil.Data;

public static class InitDB{
    private static SystemPrompt CreateDefaultPrompt(AppDbContext db){
        var prompt = new SystemPrompt()
        {
            Name = "default"
        };

        prompt.Prompts = BasePrompts.Create();

        return prompt;
    }
    
    public static void Initialize(this WebApplication app){
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.Database.EnsureCreated();
        var prompt = CreateDefaultPrompt(db);
        if(!db.Set<Settings>().Any()){
            db.Set<Settings>().Add(new Settings{DefaultPrompt = prompt, ActivePrompt = prompt});
            db.SaveChanges();
        }
    }
}
