using AutoMapper;
using Gamestore.Application.Helpers.Profiles;
using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Models.DTO;
using Gamestore.Domain.Models.DTO.Platform;

namespace Gamestore.Application.Tests.Profiles;

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
        // Arrange
        var dto = new PlatformCreateDto
        {
            Type = "Mobile",
        };

        // Act
        var entity = _mapper.Map<Platform>(dto);

        // Assert
        Assert.Equal("Mobile", entity.Type);
        Assert.Equal(Guid.Empty, entity.Id);
    }

    [Fact]
    public void Map_Platform_To_PlatformDto_Success()
    {
        // Arrange
        var entity = new Platform
        {
            Type = "Mobile",
        };

        // Act
        var dto = _mapper.Map<PlatformCreateDto>(entity);

        // Assert
        Assert.Equal("Mobile", dto.Type);
    }

    [Fact]
    public void Map_Platform_To_PlatformFullDto_Success()
    {
        // Arrange
        var entity = new Platform
        {
            Type = "Mobile",
        };

        // Act
        var dto = _mapper.Map<PlatformDto>(entity);

        // Assert
        Assert.Equal("Mobile", dto.Type);
        Assert.Equal(dto.Id, entity.Id);
    }

    [Fact]
    public void Map_PlatformUpdateDto_To_PlatformDto_Success()
    {
        // Arrange
        var dto = new PlatformUpdateDto
        {
            Type = "Mobile",
        };

        // Act
        var entity = _mapper.Map<Platform>(dto);

        // Assert
        Assert.Equal("Mobile", entity.Type);
        Assert.Equal(Guid.Empty, entity.Id);
    }
}