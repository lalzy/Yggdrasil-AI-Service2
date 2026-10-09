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

public class SystemPromptServicesTests : DatabaseSetup{
    private readonly SystemPromptServices _service;
    private readonly Faker _faker = new();
    private DbSet<SystemPrompt> SystemPrompts => Db().Set<SystemPrompt>();

    public SystemPromptServicesTests(){
        _service = new SystemPromptServices(Db());
    }

    [Fact]
    public void Create_ReturnsSavedSystemPrompt(){
        var request = AutoFaker.Generate<SystemPromptDTO.Request>();
        var result = _service.Create(request);
        result.Should().BeEquivalentTo(request, o => o.ExcludingMissingMembers());
    }

    [Fact]
    public void Create_SavesSystemPromptToDatabase(){
        var request = AutoFaker.Generate<SystemPromptDTO.Request>();
        var result = _service.Create(request);
        
        var saved = SystemPrompts.Include(s => s.Prompts).Single();
        Assert.Equivalent(result, saved, strict:true);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(5)]
    public void Get_RetrieveRequested(int index){
        var prompts = Enumerable.Range(0, 10).Select(_ => SystemPromptFactory.Create(Db())).ToList();
        var fetch = _service.Get(prompts[index].ID);

        Assert.Equivalent(prompts[index], fetch, strict:true);
    }
    
    [Fact]
    public void Get_GetNullIfNotFound(){
        var fetch = _service.Get(_faker.Random.Guid());
        Assert.Null(fetch);
    }

    [Fact]
    public void Get_GetsPromptsOrdered(){
        var Prompts = Enumerable.Range(0, 10).Select(_ => PromptFactory.CreatePrompt(Db())).OrderBy(p => p.Order).ToList();
        var systemPrompt = SystemPromptFactory.Create(Db(), Prompts);
        var fetch = _service.Get(systemPrompt.ID);
        Assert.Equal(Prompts.Select(p => p.ID), fetch.Prompts.Select(p => p.ID));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(30)]
    [InlineData(50)]
    public void GetAll_GetCountRequested(int count){
        var expected = Enumerable.Range(0, 100).Select(_ => SystemPromptFactory.Create(Db())).ToList();
        var fetched = _service.GetAll(count:count);

        var expectedByID = expected.ToDictionary(p => p.ID);
        Assert.Equal(count, fetched.Count);
        Assert.All(fetched, p => Assert.Equivalent(expectedByID[p.ID], p));
    }

    [Theory]
    [InlineData(1, 0, 1)]
    [InlineData(10, 1, 2)]
    [InlineData(25, 1, 2)]
    public void GetAll_PagesDoNotOverlap(int fetchCount, int pageA, int pageB){
        Enumerable.Range(0, 100).Select(_ => SystemPromptFactory.Create(Db())).ToList();

        var a = _service.GetAll(count:fetchCount, pageIndex:pageA).Select(p => p.ID);
        var b = _service.GetAll(count:fetchCount, pageIndex:pageB).Select(p => p.ID);

        Assert.Equal(fetchCount, a.Count());
        Assert.Equal(fetchCount, b.Count());
        Assert.Empty(a.Intersect(b));
    }

    public static TheoryData<SortOrder, Func<IEnumerable<SystemPrompt>, IEnumerable<SystemPrompt>>> SortOrders => new() {
    { SortOrder.NameAsc,  p => p.OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase).ThenBy(x => x.ID) },
        { SortOrder.NameDesc, p => p.OrderByDescending(x => x.Name, StringComparer.OrdinalIgnoreCase).ThenBy(x => x.ID) },
        { SortOrder.CreatedAsc,  p => p.OrderBy(x => x.CreatedAt).ThenBy(sp => sp.ID) },
        { SortOrder.CreatedDesc, p => p.OrderByDescending(x => x.CreatedAt).ThenBy(sp => sp.ID) },
        { SortOrder.ModifiedAsc,  p => p.OrderBy(x => x.UpdatedAt).ThenBy(sp => sp.ID) },
        { SortOrder.ModifiedDesc, p => p.OrderByDescending(x => x.UpdatedAt).ThenBy(sp => sp.ID) },
        { SortOrder.IDAsc,  p => p.OrderBy(x => x.ID) },
        { SortOrder.IDDesc, p => p.OrderByDescending(x => x.ID) },
    };

    [Theory]
    [MemberData(nameof(SortOrders))]
    public void GetAll_ReturnsSorted(SortOrder sortOrder, Func<IEnumerable<SystemPrompt>, IEnumerable<SystemPrompt>> sort){
        var expected = Enumerable.Range(0, 100).Select(_ => SystemPromptFactory.Create(Db())).ToList();

        var fetched = _service.GetAll(count: 100, sortOrder: sortOrder);

        Assert.Equal(sort(expected).Select(p => p.ID), fetched.Select(p => p.ID));
    }

    [Fact]
    public void GetAll_InvalidSortThrows(){
        Assert.Throws<ArgumentOutOfRangeException>(() => {_service.GetAll(sortOrder:(SortOrder)999);
        });
    }

    [Fact]
    public void GetAll_UsesDefaults(){
        Enumerable.Range(0, 20).Select(_ => SystemPromptFactory.Create(Db())).ToList();

        var defaults = _service.GetAll().Select(p => p.ID);
        var expected = _service.GetAll(count: 10, pageIndex:0, sortOrder:SortOrder.IDAsc).Select(p => p.ID);

        Assert.Equal(expected, defaults);
    }

    [Fact]
    public void Rename_NameChangeSuccess(){
        var original = SystemPromptFactory.Create(Db());
        string newName="";
        do{
            newName = _faker.Lorem.Word();
        }while(newName == original.Name);

        var fetched = _service.Rename(original.ID, newName);

        fetched = SystemPrompts.Single(sp => sp.ID == original.ID);

        Assert.Equal(newName, fetched.Name);
    }

    [Fact]
    public void Rename_ThrowsNullReferenceOnNotFound(){
        Assert.Throws<NullReferenceException>(() => {_service.Rename(_faker.Random.Guid(), _faker.Lorem.Word());
        });
    }
}
