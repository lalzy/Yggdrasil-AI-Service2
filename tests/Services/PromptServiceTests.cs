// SystemPromptServiceTests.cs

using FluentAssertions;
using AutoBogus;
using Bogus;
using Yggdrasil.Tests.TestUtil;
using Yggdrasil.Services;
using Yggdrasil.Models.Entities;
using Yggdrasil.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Yggdrasil.Tests.Factories;
using Yggdrasil.Models.Enums;

namespace Yggdrasil.Tests.Services;

public class PromptServicesTests : DatabaseSetup {
    private readonly PromptServices _service;
    private readonly Faker _faker = new();
    private DbSet<Prompt> Prompts => Db().Set<Prompt>();
    
    public PromptServicesTests(){
        _service = new PromptServices(Db());
    }

    [Fact]
    public void Create_ReturnsSavedPrompt(){
        var request = AutoFaker.Generate<PromptDTO.Request>();
        var result = _service.Create(request);
        result.Should().BeEquivalentTo(request, o => o.ExcludingMissingMembers());
    }

    [Fact]
    public void Create_SavesPromptToDatabase(){
        var request = AutoFaker.Generate<PromptDTO.Request>();
        var result = _service.Create(request);
        var saved = Prompts.Single();
        Assert.Equivalent(result, saved, strict: true);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(5)]
    public void Get_RetrieveRequested(int index){
        var prompts = Enumerable.Range(0, 10).Select(_ => PromptFactory.CreatePrompt(Db())).ToList();
        var fetched = _service.Get(prompts[index].ID);
        var saved = Prompts.Find(prompts[index].ID);
        Assert.Equivalent(fetched, saved, strict:true);
    }

    [Fact]
    public void Get_NullOnNotFound(){
        var result = _service.Get(_faker.Random.Guid());
        Assert.Null(result);
    }

    [Fact]
    public void Update_ReturnsUpdated(){
        var original = PromptFactory.CreatePrompt(Db());
        var updateRequest = AutoFaker.Generate<PromptDTO.UpdateRequest>();
        var fetched = _service.Update(original.ID, updateRequest);

        Assert.Equivalent(updateRequest, fetched);
        Assert.Equal(original.ID, fetched.ID);
    }

    [Fact]
    public void Update_ChangesDBEntry(){
        var original = PromptFactory.CreatePrompt(Db());
        var updateRequest = AutoFaker.Generate<PromptDTO.UpdateRequest>();
        _service.Update(original.ID, updateRequest);

        var fetched = Prompts.Find(original.ID);
        Assert.Equivalent(updateRequest, fetched);
    }

    [Fact]
    public void Update_NotFoundReturnsNull(){
        var fetch = _service.Update(_faker.Random.Guid(), AutoFaker.Generate<PromptDTO.UpdateRequest>());
        Assert.Null(fetch);
    }

    [Fact]
    public void Update_OnlyChangeRequested(){
        var target = PromptFactory.CreatePrompt(Db());
        var other = PromptFactory.CreatePrompt(Db());
        var updateRequest = AutoFaker.Generate<PromptDTO.UpdateRequest>();
        
        _service.Update(target.ID, updateRequest);

        var fetchedOther = Prompts.Find(other.ID);
        Assert.Equivalent(other, fetchedOther);
    }

    [Theory]
    [InlineData(nameof(Prompt.Name))]
    [InlineData(nameof(Prompt.Content))]
    [InlineData(nameof(Prompt.Source))]
    [InlineData(nameof(Prompt.Order))]
    [InlineData(nameof(Prompt.Active))]
    public void Update_OnlyPassedFieldsChanged(string field){
        var original = PromptFactory.CreatePrompt(Db());
        var generated = AutoFaker.Generate<PromptDTO.UpdateRequest>();
        var updateRequest = new PromptDTO.UpdateRequest();

        var requestProperty = typeof(PromptDTO.UpdateRequest).GetProperty(field);
        requestProperty.SetValue(updateRequest, requestProperty.GetValue(generated));

        _service.Update(original.ID, updateRequest);

        var fetched = Prompts.Find(original.ID);
        foreach(var property in typeof(Prompt).GetProperties()){
            if(property.Name == field)
                Assert.Equal(requestProperty.GetValue(generated), property.GetValue(fetched));
            else
                Assert.Equal(property.GetValue(original), property.GetValue(fetched));
        }
    }

    [Fact]
    public void Delete_DeletesEntry(){
        var original = PromptFactory.CreatePrompt(Db());
        _service.Delete(original.ID);
        var fetched = Prompts.Find(original.ID);
        Assert.Null(fetched);
    }


    [Fact]
    public void Delete_OnlyDeletesRequested(){
        var toDelete = PromptFactory.CreatePrompt(Db());
        var notDelete = PromptFactory.CreatePrompt(Db());

        _service.Delete(toDelete.ID);
        var fetched = Prompts.Find(notDelete.ID);
        Assert.NotNull(fetched);
    }

    [Fact]
    public void Delete_NotFoundDoesNotThrow(){
        var exception = Record.Exception(() => _service.Delete(_faker.Random.Guid()));
        Assert.Null(exception);
    }
}
