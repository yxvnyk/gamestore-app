using AutoMapper;
using Gamestore.Application.Helpers.Profiles;
using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Models.DTO.Game;
using Microsoft.Extensions.Logging.Abstractions;

namespace Gamestore.Application.Tests.Profiles;

public class GameProfileTests
{
    private readonly IMapper _mapper;

    public GameProfileTests()
    {
        var config = new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile<GenreProfile>();
            },
            NullLoggerFactory.Instance);

        config.AssertConfigurationIsValid();
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_CreateExtendedDto_To_Game()
    {
        // Arrange
        var dto = new CreateGameRequest
        {
            Game = new GameDto
            {
                Name = "Test",
                Key = "doom",
                Description = "Shooter",
                Discount = 10,
                Price = 10,
                UnitInStock = 100,
            },
            Genres = [Guid.NewGuid(), Guid.NewGuid()],
            Platforms = [Guid.NewGuid()],
        };

        // Act
        var result = _mapper.Map<Game>(dto);

        // Assert
        Assert.Equal("Test", result.Name);
        Assert.Equal("doom", result.Key);
        Assert.Equal("Shooter", result.Description);
        Assert.Equal(10, result.Discount);
        Assert.Equal(10, result.Price);
        Assert.Equal(10, result.UnitsInStock);
        Assert.Equal(2, result.GameGenres.Count);
        Assert.Single(result.GamePlatforms);

        Assert.All(result.GameGenres, g => Assert.NotEqual(Guid.Empty, g.GenreId));
        Assert.All(result.GamePlatforms, p => Assert.NotEqual(Guid.Empty, p.PlatformId));
    }

    [Fact]
    public void Map_UpdateExtendedDto_To_Game()
    {
        // Arrange
        var dto = new UpdateGameRequest
        {
            Game = new GameUpdateDto
            {
                Name = "Updated",
                Key = "updated-key",
                Description = "Updated desc",
                Discount = 20,
                Price = 20,
                UnitInStock = 200,
            },
        };

        var game = new Game
        {
            Id = Guid.NewGuid(),
            Name = "Old",
            Key = "old-key",
            Description = "Old desc",
            Discount = 10,
            Price = 10,
            UnitsInStock = 100,
        };

        // Act
        _mapper.Map(dto, game);

        // Assert
        Assert.Equal("Updated", game.Name);
        Assert.Equal("updated-key", game.Key);
        Assert.Equal("Updated desc", game.Description);
    }

    [Fact]
    public void Map_Game_To_GameDto()
    {
        // Arrange
        var game = new Game
        {
            Id = Guid.NewGuid(),
            Name = "Game 1",
            Key = "g1",
            Description = "desc",
        };

        // Act
        var dto = _mapper.Map<GameDto>(game);

        // Assert
        Assert.Equal(game.Id.ToString(), dto.Id);
        Assert.Equal(game.Name, dto.Name);
        Assert.Equal(game.Key, dto.Key);
        Assert.Equal(game.Description, dto.Description);
    }

    [Fact]
    public void Map_UpdateExtendedDto_With_Null_DoesNotOverrideExistingValues()
    {
        // Arrange
        var dto = new UpdateGameRequest
        {
            Game = new GameUpdateDto
            {
                Name = null,
                Key = null,
                Description = "New Desc",
            },
        };

        var game = new Game
        {
            Name = "Old Name",
            Key = "old-key",
            Description = "Old Desc",
        };

        // Act
        _mapper.Map(dto, game);

        // Assert
        Assert.Equal("Old Name", game.Name);
        Assert.Equal("old-key", game.Key);
        Assert.Equal("New Desc", game.Description);
    }

    [Fact]
    public void Map_CreateExtendedDto_Null_GameProperties_SkipAssignment()
    {
        // Arrange
        var dto = new CreateGameRequest
        {
            Game = new GameDto
            {
                Name = null,
                Key = null,
                Description = null,
                Discount = 0,
                Price = 0,
                UnitInStock = 0,
            },
            Genres = [Guid.NewGuid(), Guid.NewGuid()],
            Platforms = [Guid.NewGuid(), Guid.NewGuid()],
        };

        // Act
        var result = _mapper.Map<Game>(dto);

        // Assert
        Assert.Null(result.Name);
        Assert.Null(result.Key);
        Assert.Equal(0, result.Discount);
        Assert.Equal(0, result.Price);
        Assert.Equal(0, result.UnitsInStock);
    }
}