// AppDbContext.cs

using System.Reflection;
using Yggdrasil.Models.Entities;

namespace Yggdrasil.Data;

public class AppDbContext : DbContext{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    ///<summary>Register all Models as DB Tables automatically</summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder){
        var entityTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.Namespace == "Yggdrasil.Models.Entities" && t != typeof(Prompt));
        foreach(var type in entityTypes){
            modelBuilder.Entity(type);
        }

        modelBuilder.Entity<SystemPrompt>().OwnsMany(sp => sp.Prompts, p => p.ToJson());

        base.OnModelCreating(modelBuilder);
    }
}
