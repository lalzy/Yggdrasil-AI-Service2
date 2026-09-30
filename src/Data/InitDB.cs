// InitDb.cs

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Yggdrasil.Models.Entities;

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

    private static SystemPrompt CreateDefaultPrompt(){
        var prompt = new SystemPrompt()
        {
            Name = "default"
        };

        prompt.Prompts.Add(new Prompt { Name = "Main", Content = MAINCONTENT, Order=0, Active=true });
        prompt.Prompts.Add(new Prompt { Name = "World", Source=SourceType.World, Order=1, Active=true});
        prompt.Prompts.Add(new Prompt { Name = "Characters", Source=SourceType.Persona, Order=2, Active=true});
        prompt.Prompts.Add(new Prompt { Name = "Characters", Source=SourceType.Characters, Order=3, Active=true});
        prompt.Prompts.Add(new Prompt { Name = "ChatHistory", Source=SourceType.History, Order=4, Active=true});

        return prompt;
    }
    
    public static void Initialize(this WebApplication app){
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.Database.EnsureCreated();
        var prompt = CreateDefaultPrompt();
        if(!db.Set<Settings>().Any()){
            db.Set<Settings>().Add(new Settings{DefaultPrompt = prompt, ActivePrompt = prompt});
            db.SaveChanges();
        }
    }
}
