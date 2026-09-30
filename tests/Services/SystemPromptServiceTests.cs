// SystemPromptServiceTests.cs

using Bogus;
using Yggdrasil.Tests.TestUtil;
using Yggdrasil.Services;
using Yggdrasil.Models.Entities;
using Yggdrasil.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Yggdrasil.Tests.Services;

public class SystemPromptServicesTests : DatabaseSetup{
    private readonly SystemPromptServices _service;
    private readonly Faker _faker = new();

    public SystemPromptServicesTests(){
        _service = new SystemPromptServices(Db());
    }

    [Fact]
    public void Crate_ReturnsSavedSystemPrompt(){
        var request = new SystemPromptDTO.Request(_faker.Lorem.Word(), new List<Prompt>());
        var result = _service.Create(request);
        var saved = Db().Set<SystemPrompt>().Single();
        Assert.Equal(saved.ID, result.ID);
    }

    [Fact]
    public void Create_SavesSystemPromptToDatabase(){
        var name = _faker.Lorem.Word();
        
        var request = new SystemPromptDTO.Request(name, new List<Prompt> {new Prompt()});

        _service.Create(request);

        var saved = Db().Set<SystemPrompt>().Single();
        Assert.Equal(name, saved.Name);
    }

    [Fact]
    public void Create_SavesPrompts(){
        var prompts = new List<Prompt> { new Prompt(), new Prompt() };
        var request = new SystemPromptDTO.Request(_faker.Lorem.Word(), prompts);

        _service.Create(request);
        var saved = Db().Set<SystemPrompt>().Include(s => s.Prompts).Single();
        Assert.Equal(2, saved.Prompts.Count);
    }

    [Fact]
    public void Create_SavesPromptFields(){
        var prompt = new Prompt{
            Name = _faker.Lorem.Word(),
            Content = _faker.Lorem.Sentence(),
            Source = SourceType.User,
            Order = 3,
            Active=true
        };

        var request = new SystemPromptDTO.Request(_faker.Lorem.Word(), new List<Prompt> { prompt });
        _service.Create(request);

        var saved = Db().Set<SystemPrompt>().Include(s => s.Prompts).Single().Prompts.Single();
        Assert.Equivalent(saved, prompt);
    }
}
