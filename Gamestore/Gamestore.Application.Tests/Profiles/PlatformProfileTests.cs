using AutoMapper;
using Gamestore.Application.Helpers.Profiles;
using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Models.DTO;

namespace Gamestore.Application.UnitTests.Profiles;

public class PlatformProfileTests
{
    private readonly IMapper _mapper;

    public PlatformProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PlatformProfile>();
        });

        config.AssertConfigurationIsValid();
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_PlatformDto_To_Platform_Success()
    {
        var dto = new PlatformDto
        {
            Type = "Mobile",
        };

        var entity = _mapper.Map<Platform>(dto);

        Assert.Equal("Mobile", entity.Type);
        Assert.Equal(Guid.Empty, entity.Id);
    }

    [Fact]
    public void Map_Platform_To_PlatformDto_Success()
    {
        var entity = new Platform
        {
            Type = "Mobile",
        };

        var dto = _mapper.Map<PlatformDto>(entity);

        Assert.Equal("Mobile", dto.Type);
    }

    [Fact]
    public void Map_Platform_To_PlatformFullDto_Success()
    {
        var entity = new Platform
        {
            Type = "Mobile",
        };

        var dto = _mapper.Map<PlatformFullDto>(entity);

        Assert.Equal("Mobile", dto.Type);
        Assert.Equal(dto.Id, entity.Id);
    }

    [Fact]
    public void Map_PlatformUpdateDto_To_PlatformDto_Success()
    {
        var dto = new PlatformUpdateDto
        {
            Type = "Mobile",
        };

        var entity = _mapper.Map<Platform>(dto);

        Assert.Equal("Mobile", entity.Type);
        Assert.Equal(Guid.Empty, entity.Id);
    }
}
