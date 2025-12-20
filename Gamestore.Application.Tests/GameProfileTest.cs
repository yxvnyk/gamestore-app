using AutoMapper;
using Gamestore.Application.Exceptions;
using Gamestore.Application.Helpers.Profiles;
using Gamestore.Application.Services;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Models.DTO;
using GameStore.Application.Helpers;
using GameStore.Application.Helpers.Interfaces;
using Moq;

namespace Gamestore.Application.Tests;

public class GameProfileTests
{
    private readonly IMapper _mapper;

    public GameProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<GameProfile>();
        });

        config.AssertConfigurationIsValid();
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_CreateExtendedDto_To_Game()
    {
        var dto = new GameCreateExtendedDto
        {
            Game = new GameDto
            {
                Name = "Test",
                Key = "doom",
                Description = "Shooter"
            },
            Genres = new Guid[] { Guid.NewGuid(), Guid.NewGuid() },
            Platforms = new Guid[] {Guid.NewGuid() }
        };

        var result = _mapper.Map<Game>(dto);

        Assert.Equal("Test", result.Name);
        Assert.Equal("doom", result.Key);
        Assert.Equal("Shooter", result.Description);
        Assert.Equal(2, result.GameGenres.Count);
        Assert.Single(result.GamePlatforms);

        Assert.All(result.GameGenres, g => Assert.NotEqual(Guid.Empty, g.GenreId));
        Assert.All(result.GamePlatforms, p => Assert.NotEqual(Guid.Empty, p.PlatformId));
    }

    [Fact]
    public void Map_UpdateExtendedDto_To_Game()
    {
        var dto = new GameUpdateExtendedDto
        {
            Game = new GameUpdateDto
            {
                Name = "Updated",
                Key = "updated-key",
                Description = "Updated desc"
            }
        };

        var game = new Game
        {
            Id = Guid.NewGuid(),
            Name = "Old",
            Key = "old-key",
            Description = "Old desc"
        };

        _mapper.Map(dto, game);

        Assert.Equal("Updated", game.Name);
        Assert.Equal("updated-key", game.Key);
        Assert.Equal("Updated desc", game.Description);
    }

    [Fact]
    public void Map_Game_To_GameDto()
    {
        var game = new Game
        {
            Id = Guid.NewGuid(),
            Name = "Game 1",
            Key = "g1",
            Description = "desc"
        };

        var dto = _mapper.Map<GameDto>(game);

        Assert.Equal(game.Id, dto.Id);
        Assert.Equal(game.Name, dto.Name);
        Assert.Equal(game.Key, dto.Key);
        Assert.Equal(game.Description, dto.Description);
    }

    [Fact]
    public void Map_UpdateExtendedDto_With_Null_DoesNotOverrideExistingValues()
    {
        var dto = new GameUpdateExtendedDto
        {
            Game = new GameUpdateDto
            {
                Name = null, 
                Key = null,  
                Description = "New Desc"
            }
        };

        var game = new Game
        {
            Name = "Old Name",
            Key = "old-key",
            Description = "Old Desc"
        };

        _mapper.Map(dto, game);

        Assert.Equal("Old Name", game.Name);  
        Assert.Equal("old-key", game.Key);    
        Assert.Equal("New Desc", game.Description);
    }

    [Fact]
    public void Map_CreateExtendedDto_Null_GameProperties_SkipAssignment()
    {
        var dto = new GameCreateExtendedDto
        {
            Game = new GameDto
            {
                Name = null,
                Key = null,
                Description = null
            },
            Genres = new Guid[] { Guid.NewGuid(), Guid.NewGuid() },
            Platforms = new Guid[] { Guid.NewGuid(), Guid.NewGuid() }
        };

        var result = _mapper.Map<Game>(dto);

        Assert.Null(result.Name);
        Assert.Null(result.Key);
        Assert.Null(result.Description);
    }
}
