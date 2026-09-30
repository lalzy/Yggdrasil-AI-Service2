// SystemPromptServiceTests.cs

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

    [Fact]
    public void Get_RetrieveRequested(){
        var prompt = SystemPromptFactory.Create(Db());
        var fetch = _service.Get(prompt.ID);

        Assert.Equivalent(prompt, fetch);
    }
    
    [Fact]
    public void Get_GetNullIfNotFound(){
        var fetch = _service.Get(_faker.Random.Guid());
        Assert.Null(fetch);
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
    public void Update_NameChangeSuccess(){
        var original = SystemPromptFactory.Create(Db());
        var request = new SystemPromptDTO.UpdateRequest{Name=_faker.Lorem.Word()};
        
        original.Name = request.Name; // Add request name to original.

        var fetched = _service.Update(original.ID, request);

        Assert.Equivalent(original, fetched);

        fetched = Db().Set<SystemPrompt>().Include(sp => sp.Prompts).Single(sp => sp.ID == original.ID);

        Assert.Equivalent(original, fetched);
    }

    [Fact]
    public void Update_PromptChangeSuccess(){
        
    }
}
