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
        var saved = Db().Set<Prompt>().Single();
        Assert.Equivalent(result, saved, strict: true);
    }
}
