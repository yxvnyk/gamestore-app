using AutoMapper;
using Gamestore.Application.Helpers.Profiles;
using Gamestore.Application.Services;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.DataAccess.Wrappers;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Generators;
using Gamestore.Domain.Interfaces;
using Gamestore.Domain.Models;
using Gamestore.Domain.Models.DTO.Game;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Gamestore.Application.Tests.Services;

public class GameServiceTest
{
    private readonly IMapper _mapper;

    private readonly Mock<IGameRepository> _mockGameRepo = new();
    private readonly Mock<INorthwindProductRepository> _mockNorthwindRepo = new();
    private readonly Mock<IGenreRepository> _mockGenreRepo = new();
    private readonly Mock<IPlatformRepository> _mockPlatformRepo = new();
    private readonly Mock<IPublisherRepository> _mockPublisherRepo = new();
    private readonly Mock<IKeyGenerator> _mockKeyGen = new();
    private readonly Mock<ILogger<GameService>> _mockLogger = new();
    private readonly Mock<IMapper> _mockMapper = new();

    public GameServiceTest()
    {
        var config = new MapperConfiguration(
            cfg =>
        {
            cfg.AddProfile<GameProfile>();
        },
            NullLoggerFactory.Instance);

        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task UpdateGameAsync_GameNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var gameDto = new UpdateGameRequest
        {
            Game = new GameUpdateDto
            {
                Id = gameId.ToString(),
                Name = "Test Game",
                Description = "Test Description",
                Key = "test-game",
            },
            Genres = [Guid.NewGuid().ToString(), Guid.NewGuid().ToString()],
            Platforms = [Guid.NewGuid()],
        };

        _mockGameRepo.Setup(repo => repo.GetGameWithJoinsAsync(gameId)).ReturnsAsync((Game?)null);

        var gameService = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await gameService.UpdateGameAsync(gameDto));
    }

    [Fact]
    public async Task UpdateGameAsync_ValidModel_UpdatesEntity()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var genres = new string[] { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };
        var platforms = new Guid[] { Guid.NewGuid(), Guid.NewGuid() };
        var gameDto = new UpdateGameRequest
        {
            Game = new GameUpdateDto
            {
                Id = gameId.ToString(),
                Name = "Test Game",
                Description = "Test Description",
                Key = "test-game",
            },
            Genres = genres,
            Platforms = platforms,
        };

        var entity = new Game
        {
            Id = gameId,
            GameGenres = [],
            GamePlatforms = [],
        };

        _mockGameRepo.Setup(repo => repo.GetGameWithJoinsAsync(gameId)).ReturnsAsync(entity);
        _mockGenreRepo.Setup(repo => repo.GenreExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);
        _mockPlatformRepo.Setup(repo => repo.PlatformExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

        var gameService = CreateService();

        // Act
        await gameService.UpdateGameAsync(gameDto);

        // Assert
        _mockMapper.Verify(m => m.Map(gameDto, entity), Times.Once);
        _mockGameRepo.Verify(r => r.UpdateGameAsync(entity), Times.Once);

        Assert.All(entity.GameGenres, gg => Assert.Contains(gg.GenreId.ToString(), genres));
        Assert.All(entity.GamePlatforms, gp => Assert.Contains(gp.PlatformId, platforms));
    }

    [Fact]
    public async Task GetGameAsync_ByKey_GameNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var gameKey = "non-existent-game";
        _mockGameRepo.Setup(repo => repo.GetGameByKeyAsync(gameKey)).ReturnsAsync((Game?)null);
        var gameService = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await gameService.GetAsync(gameKey));
        _mockGameRepo.Verify(r => r.GetGameByKeyAsync(gameKey), Times.Once);
    }

    [Fact]
    public async Task GetGameAsync_ByKey_GameFound_ReturnGameWithTheSameKey()
    {
        // Arrange
        var gameKey = "exist-game";
        var gameId = Guid.NewGuid();
        var gameName = "name";
        var gameDescription = "Exist Description";

        var game = new Game
        {
            Id = gameId,
            Name = gameName,
            Description = gameDescription,
            Key = gameKey,
        };
        var gameDto = new GameDto
        {
            Id = gameId.ToString(),
            Name = gameName,
            Description = gameDescription,
            Key = gameKey,
        };

        _mockGameRepo.Setup(repo => repo.GetGameByKeyAsync(gameKey)).ReturnsAsync(game);
        _mockMapper.Setup(m => m.Map<GameDto>(game)).Returns(gameDto);
        var gameService = CreateService();

        // Act
        var result = await gameService.GetAsync(gameKey);

        // Assert
        Assert.Equal(gameDto.Id, result.Id);
        Assert.Equal(gameDto.Name, result.Name);
        Assert.Equal(gameDto.Description, result.Description);
        Assert.Equal(gameDto.Key, result.Key);
    }

    [Fact]
    public async Task GetGameAsync_ById_ByKeyGameNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var identity = new Identity(gameId, null);
        _mockGameRepo.Setup(repo => repo.GetByIdAsync(gameId)).ReturnsAsync((Game?)null);
        var gameService = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await gameService.GetByIdAsync(identity));
        _mockGameRepo.Verify(r => r.GetByIdAsync(gameId), Times.Once);
    }

    [Fact]
    public async Task GetGameAsync_ById_GameFound_ReturnGameWithTheSameKey()
    {
        // Arrange
        var gameKey = "exist-game";
        var gameId = Guid.NewGuid();
        var identity = new Identity(gameId, null);
        var gameName = "name";
        var gameDescription = "Exist Description";

        var game = CreateGame(
            name: gameName,
            key: gameKey,
            description: gameDescription);
        game.Id = gameId;

        var gameDto = CreateGameDto(game);

        _mockGameRepo.Setup(repo => repo.GetByIdAsync(gameId)).ReturnsAsync(game);
        _mockMapper.Setup(m => m.Map<GameDto>(game)).Returns(gameDto);
        var gameService = CreateService();

        // Act
        var result = await gameService.GetByIdAsync(identity);

        // Assert
        Assert.Equal(gameDto.Id, result.Id);
        Assert.Equal(gameDto.Name, result.Name);
        Assert.Equal(gameDto.Description, result.Description);
        Assert.Equal(gameDto.Key, result.Key);
    }

    [Fact]
    public async Task GetGamesByPlatformAsync_NoGames_ReturnEmptyCollection()
    {
        // Arrange
        var platformId = Guid.NewGuid();
        _mockGameRepo.Setup(r => r.GetGamesByPlatformAsync(platformId))
            .ReturnsAsync([]);

        var service = CreateService();

        // Act
        var result = await service.GetByPlatformAsync(platformId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _mockGameRepo.Verify(r => r.GetGamesByPlatformAsync(platformId), Times.Once);
    }

    [Fact]
    public async Task GetGamesByPlatformAsync_CorrectCollection_ReturnGamesCollection()
    {
        // Arrange
        var platformId = Guid.NewGuid();
        var platforms = new List<GamePlatform>
        {
            new() { PlatformId = platformId },
        };

        var game1 = new Game { Id = Guid.NewGuid(), Name = "Game1", Description = "Desc1", Key = "key1", GamePlatforms = platforms };
        var game2 = new Game { Id = Guid.NewGuid(), Name = "Game2", Description = "Desc2", Key = "key2", GamePlatforms = platforms };
        var game3 = new Game { Id = Guid.NewGuid(), Name = "Game3", Description = "Desc3", Key = "key3", GamePlatforms = platforms };

        var games = new List<Game> { game1, game2, game3 };

        var dto1 = new GameDto { Id = game1.Id.ToString(), Name = game1.Name, Description = game1.Description, Key = game1.Key, };
        var dto2 = new GameDto { Id = game2.Id.ToString(), Name = game2.Name, Description = game2.Description, Key = game2.Key };
        var dto3 = new GameDto { Id = game3.Id.ToString(), Name = game3.Name, Description = game3.Description, Key = game3.Key };

        _mockGameRepo.Setup(r => r.GetGamesByPlatformAsync(platformId)).ReturnsAsync(games);
        _mockMapper.Setup(m => m.Map<GameDto>(game1)).Returns(dto1);
        _mockMapper.Setup(m => m.Map<GameDto>(game2)).Returns(dto2);
        _mockMapper.Setup(m => m.Map<GameDto>(game3)).Returns(dto3);

        var gameService = CreateService();

        // Act
        var result = await gameService.GetByPlatformAsync(platformId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);

        Assert.Contains(result, r => r.Id == dto1.Id && r.Name == dto1.Name);
        Assert.Contains(result, r => r.Id == dto2.Id && r.Name == dto2.Name);
        Assert.Contains(result, r => r.Id == dto3.Id && r.Name == dto3.Name);

        _mockGameRepo.Verify(r => r.GetGamesByPlatformAsync(platformId), Times.Once);
        _mockMapper.Verify(m => m.Map<GameDto>(It.IsAny<Game>()), Times.Exactly(3));
    }

    [Fact]
    public async Task GetGamesByGenreAsync_NoGames_ReturnEmptyCollection()
    {
        // Arrange
        var platformId = Guid.NewGuid();
        var identity = new Identity(platformId, null);
        _mockGameRepo.Setup(r => r.GetByGenreAsync(platformId))
            .ReturnsAsync([]);

        var service = CreateService();

        // Act
        var result = await service.GetByGenreAsync(identity);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _mockGameRepo.Verify(r => r.GetByGenreAsync(platformId), Times.Once);
    }

    [Fact]
    public async Task GetGamesByGenreAsync_CorrectCollection_ReturnGamesCollection()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var identity = new Identity(genreId, null);
        var genres = new List<GameGenre>
        {
            new() { GenreId = genreId },
        };

        var game1 = new Game { Id = Guid.NewGuid(), Name = "Game1", Description = "Desc1", Key = "key1", GameGenres = genres };
        var game2 = new Game { Id = Guid.NewGuid(), Name = "Game2", Description = "Desc2", Key = "key2", GameGenres = genres };
        var game3 = new Game { Id = Guid.NewGuid(), Name = "Game3", Description = "Desc3", Key = "key3", GameGenres = genres };

        var games = new List<Game> { game1, game2, game3 };

        var dto1 = new GameDto { Id = game1.Id.ToString(), Name = game1.Name, Description = game1.Description, Key = game1.Key, };
        var dto2 = new GameDto { Id = game2.Id.ToString(), Name = game2.Name, Description = game2.Description, Key = game2.Key };
        var dto3 = new GameDto { Id = game3.Id.ToString(), Name = game3.Name, Description = game3.Description, Key = game3.Key };

        _mockGameRepo.Setup(r => r.GetByGenreAsync(genreId)).ReturnsAsync(games);
        _mockMapper.Setup(m => m.Map<GameDto>(game1)).Returns(dto1);
        _mockMapper.Setup(m => m.Map<GameDto>(game2)).Returns(dto2);
        _mockMapper.Setup(m => m.Map<GameDto>(game3)).Returns(dto3);

        var gameService = CreateService();

        // Act
        var result = await gameService.GetByGenreAsync(identity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);

        Assert.Contains(result, r => r.Id == dto1.Id && r.Name == dto1.Name);
        Assert.Contains(result, r => r.Id == dto2.Id && r.Name == dto2.Name);
        Assert.Contains(result, r => r.Id == dto3.Id && r.Name == dto3.Name);

        // Verify repository and mapper calls
        _mockGameRepo.Verify(r => r.GetByGenreAsync(genreId), Times.Once);
        _mockMapper.Verify(m => m.Map<GameDto>(It.IsAny<Game>()), Times.Exactly(3));
    }

    [Fact]
    public async Task GetAllGamesAsync_NoGames_ReturnEmptyCollection()
    {
        // Arrange
        var request = new GetGamesRequest();
        var pagedList = new[]
        {
            new GameWithStats
            {
                Game = null,
                CreatedDate = null,
                CommentCount = 0,
            },
            new GameWithStats
            {
                Game = null,
                CommentCount = 0,
                CreatedDate = null,
            },
        };

        _mockGameRepo.Setup(r => r.GetAllGamesAsync(request))
            .ReturnsAsync(pagedList);

        var service = CreateService();

        // Act
        var result = await service.GetAllGamesAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Games);
        _mockGameRepo.Verify(r => r.GetAllGamesAsync(request), Times.Once);
    }

    [Fact]
    public async Task GetAllGamesAsync_CorrectCollection_ReturnGamesCollection()
    {
        // Arrange
        var request = new GetGamesRequest();
        var genreId = Guid.NewGuid();
        var genres = new List<GameGenre>
        {
            new() { GenreId = genreId },
        };

        var game1 = new Game { Id = Guid.NewGuid(), Name = "Game1", Description = "Desc1", Key = "key1", GameGenres = genres };
        var game2 = new Game { Id = Guid.NewGuid(), Name = "Game2", Description = "Desc2", Key = "key2", GameGenres = genres };
        var game3 = new Game { Id = Guid.NewGuid(), Name = "Game3", Description = "Desc3", Key = "key3", GameGenres = genres };

        var games = new List<Game> { game1, game2, game3 };
        var pagedList = new[]
        {
            new GameWithStats
            {
                Game = game1,
                CreatedDate = null,
                CommentCount = 0,
            },
            new GameWithStats
            {
                Game = game2,
                CommentCount = 0,
                CreatedDate = null,
            },
            new GameWithStats
            {
                Game = game3,
                CommentCount = 0,
                CreatedDate = null,
            },
        };

        var dto1 = new GameDto { Id = game1.Id.ToString(), Name = game1.Name, Description = game1.Description, Key = game1.Key, };
        var dto2 = new GameDto { Id = game2.Id.ToString(), Name = game2.Name, Description = game2.Description, Key = game2.Key };
        var dto3 = new GameDto { Id = game3.Id.ToString(), Name = game3.Name, Description = game3.Description, Key = game3.Key };

        _mockGameRepo.Setup(r => r.GetAllGamesAsync(request)).ReturnsAsync(pagedList);
        _mockMapper.Setup(m => m.Map<GameDto>(game1)).Returns(dto1);
        _mockMapper.Setup(m => m.Map<GameDto>(game2)).Returns(dto2);
        _mockMapper.Setup(m => m.Map<GameDto>(game3)).Returns(dto3);

        var gameService = CreateService();

        // Act
        var result = await gameService.GetAllGamesAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Games.Count());

        Assert.Contains(result.Games, r => r.Id == dto1.Id && r.Name == dto1.Name);
        Assert.Contains(result.Games, r => r.Id == dto2.Id && r.Name == dto2.Name);
        Assert.Contains(result.Games, r => r.Id == dto3.Id && r.Name == dto3.Name);

        // Verify repository and mapper calls
        _mockGameRepo.Verify(r => r.GetAllGamesAsync(request), Times.Once);
        _mockMapper.Verify(m => m.Map<GameDto>(It.IsAny<Game>()), Times.Exactly(3));
    }

    [Fact]
    public async Task CreateGameAsync_GenreNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var gameDto = new CreateGameRequest
        {
            Game = new GameDto
            {
                Id = gameId.ToString(),
                Name = "Test Game",
                Description = "Test Description",
                Key = "test-game",
            },
            Genres = [Guid.NewGuid(), Guid.NewGuid()],
            Platforms = [Guid.NewGuid()],
        };

        _mockGenreRepo.Setup(r => r.GenreExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        var gameService = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await gameService.CreateGameAsync(gameDto));
    }

    [Fact]
    public async Task CreateGameAsync_PatformNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var gameDto = new CreateGameRequest
        {
            Game = new GameDto
            {
                Id = gameId.ToString(),
                Name = "Test Game",
                Description = "Test Description",
                Key = "test-game",
            },
            Genres = [Guid.NewGuid(), Guid.NewGuid()],
            Platforms = [Guid.NewGuid()],
        };

        _mockPlatformRepo.Setup(r => r.PlatformExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        var gameService = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await gameService.CreateGameAsync(gameDto));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task CreateGameAsync_EmptyKey_GenerateNewKeyAndCreateEntity(string key)
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var genreId = Guid.NewGuid();
        var platformId = Guid.NewGuid();

        var gameDto = new CreateGameRequest
        {
            Game = new GameDto
            {
                Id = gameId.ToString(),
                Name = "game",
                Key = key,
            },
            Genres = [genreId],
            Platforms = [platformId],
        };

        var entity1 = _mapper.Map<Game>(gameDto);

        _mockMapper.Setup(m => m.Map<Game>(gameDto)).Returns(entity1);

        _mockGenreRepo.Setup(r => r.GenreExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);
        _mockPlatformRepo.Setup(r => r.PlatformExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);
        _mockPublisherRepo.Setup(r => r.PublisherExistAsync(It.IsAny<Guid>())).ReturnsAsync(true);

        _mockKeyGen.Setup(g => g.GenerateUniqueKeyAsync((IUniqueKeyRepository)_mockGameRepo.Object, gameDto.Game.Name)).ReturnsAsync("game1");

        var gameService = CreateService();

        // Act
        await gameService.CreateGameAsync(gameDto);

        // Assert
        _mockKeyGen.Verify(m => m.GenerateUniqueKeyAsync((IUniqueKeyRepository)_mockGameRepo.Object, "game"), Times.Once);

        _mockMapper.Verify(m => m.Map<Game>(gameDto), Times.Once);

        _mockGameRepo.Verify(
            r => r.CreateGameAsync(It.IsAny<Game>()),
            Times.Once);
    }

    [Fact]
    public async Task DeleteByKeyAsync_RepositoryReturnsTrue_ReturnsTrue()
    {
        // Arrange
        var key = "game-key";
        _mockGameRepo
            .Setup(r => r.DeleteByKeyAsync(key))
            .ReturnsAsync(true);

        var service = CreateService();

        // Act
        var result = await service.DeleteByKeyAsync(key);

        // Assert
        Assert.True(result);
        _mockGameRepo.Verify(r => r.DeleteByKeyAsync(key), Times.Once);
    }

    [Fact]
    public async Task DeleteByKeyAsync_RepositoryReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var key = "missing-key";
        _mockGameRepo
            .Setup(r => r.DeleteByKeyAsync(key))
            .ReturnsAsync(false);

        var service = CreateService();

        // Act
        var result = await service.DeleteByKeyAsync(key);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetTotalGamesCountAsync_ReturnsRepositoryValue()
    {
        // Arrange
        _mockGameRepo
            .Setup(r => r.GetTotalGamesCountAsync())
            .ReturnsAsync(42);

        var service = CreateService();

        // Act
        var result = await service.GetTotalGamesCountAsync();

        // Assert
        Assert.Equal(42, result);
        _mockGameRepo.Verify(r => r.GetTotalGamesCountAsync(), Times.Once);
    }

    private static GameDto CreateGameDto(Game game)
        => new()
        {
            Id = game.Id.ToString(),
            Name = game.Name,
            Description = game.Description,
            Key = game.Key,
        };

    private static Game CreateGame(
    string name = "Game",
    string key = "key",
    string description = "Desc")
    => new()
    {
        Id = Guid.NewGuid(),
        Name = name,
        Key = key,
        Description = description,
    };

    private GameService CreateService() => new(
    _mockGameRepo.Object,
    _mockNorthwindRepo.Object,
    _mockGenreRepo.Object,
    _mockPlatformRepo.Object,
    _mockPublisherRepo.Object,
    _mockKeyGen.Object,
    _mockMapper.Object,
    _mockLogger.Object);
}