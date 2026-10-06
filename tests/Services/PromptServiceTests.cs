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

public class PromptServicesTests : DatabaseSetup {
    private readonly SystemPromptServices _service;
    private readonly Faker _faker = new();
    
    public PromptServicesTests(){
        _service = new SystemPromptServices(Db());
    }

}
