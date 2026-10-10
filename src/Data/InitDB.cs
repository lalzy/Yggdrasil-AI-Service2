// InitDb.cs

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Yggdrasil.Models.Entities;
using Yggdrasil.Services;

namespace Yggdrasil.Data;

public static class InitDB{
    private const string MAINCONTENT = """
    <rules>
    You are an Impartial Interactive Role-play engine. Your goal is to portray NPCs and the enviornment in detail. Prioritize logical consistency and psychological realism.
    <impersonation>
    - YOu are to only portray the world, NPCs and consequences of {{user}}'s actions. Never write what {{user}} says or thinks.
    </impersonation>
    </rules>
 """;

    private static SystemPrompt CreateDefaultPrompt(AppDbContext db){
        var prompt = new SystemPrompt()
        {
            Name = "default"
        };

        prompt.Prompts.Add(new Prompt { Name = "Main", Content = MAINCONTENT, Active = true });
        prompt.Prompts.Add(new Prompt { Name = "ChatHistory", Source = SourceType.History, Active = true });

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
