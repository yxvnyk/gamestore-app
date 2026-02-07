using System.Text.Json;
using GameStore.Application.Helpers.Interfaces;
using Gamestore.Application.Services.Interfaces;
using Gamestore.Domain.Models.DTO;
using Gamestore.Domain.Models.DTO.Game;
using Gamestore.Domain.Models.DTO.Genre;
using Gamestore.Domain.Models.DTO.Platform;
using Gamestore.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Gamestore.WebApi.Tests.Controllers;

public class GameControllerTests
{
    private readonly Mock<IGameService> _mockGameService = new();
    private readonly Mock<IGenreService> _mockGenreService = new();
    private readonly Mock<IPlatformService> _mockPlatformService = new();
    private readonly Mock<IPublisherService> _mockPublisherService = new();
    private readonly Mock<IGenerateGameFile> _mockGenerateGameFile = new();

    private readonly List<GameDto> _expectedGameDtos =
    [
        new GameDto
        {
            Id = Guid.NewGuid(),
            Key = "game-key-123",
            Name = "Sample Game 1",
            Description = "Description for Sample Game 1",
        },
        new GameDto
        {
            Id = Guid.NewGuid(),
            Key = "game-key-456",
            Name = "Sample Game 2",
            Description = "Description for Sample Game 2",
        },
        new GameDto
        {
            Id = Guid.NewGuid(),
            Key = "game-key-789",
            Name = "Sample Game 3",
            Description = "Description for Sample Game 3",
        },
    ];

    private readonly List<GenreDto> _expectedGenreDtos =
    [
        new GenreDto
        {
            Id = Guid.NewGuid(),
            Name = "Action",
        },
        new GenreDto
        {
            Id = Guid.NewGuid(),
            Name = "Adventure",
        },
        new GenreDto
        {
            Id = Guid.NewGuid(),
            Name = "RPG",
        }
    ];

    private readonly List<PlatformDto> _expectedPlatformDtos =
    [
        new PlatformDto
        {
            Id = Guid.NewGuid(),
            Type = "PC",
        },
        new PlatformDto
        {
            Id = Guid.NewGuid(),
            Type = "Console",
        },
        new PlatformDto
        {
            Id = Guid.NewGuid(),
            Type = "Mobile",
        }
    ];

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
    };

    [Fact]
    public async Task CreateGameReturnOk()
    {
        // Arrange
        var dto = new CreateGameRequest();
        var controller = CreateController();

        // Act
        var result = await controller.CreateGame(dto);

        // Assert
        _mockGameService.Verify(s => s.CreateGameAsync(dto), Times.Once);
        Assert.IsType<OkResult>(result);
    }

    [Theory]
    [InlineData("game-key-123")]
    public async Task GetGameByKeyReturnOk(string key)
    {
        // Arrange
        var expectedGameDto = new GameDto
        {
            Key = key,
            Description = "Sample Game",
            Name = "Sample Game Name",
        };

        _mockGameService
            .Setup(s => s.GetGameAsync(key))
            .ReturnsAsync(expectedGameDto);

        var controller = CreateController();

        // Act
        var result = await controller.GetGameByKey(key);

        // Assert
        _mockGameService.Verify(s => s.GetGameAsync(key), Times.Once);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGame = Assert.IsType<GameDto>(okResult.Value);

        Assert.Equal(expectedGameDto.Id, returnedGame.Id);
        Assert.Equal(expectedGameDto.Name, returnedGame.Name);
        Assert.Equal(expectedGameDto.Key, returnedGame.Key);
    }

    [Fact]
    public async Task GetGameByIdReturnOk()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expectedGameDto = new GameDto
        {
            Key = "key",
            Description = "Sample Game",
            Name = "Sample Game Name",
        };

        _mockGameService
            .Setup(s => s.GetGameAsync(id))
            .ReturnsAsync(expectedGameDto);

        var controller = CreateController();

        // Act
        var result = await controller.GetGameById(id);

        // Assert
        _mockGameService.Verify(s => s.GetGameAsync(id), Times.Once);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGame = Assert.IsType<GameDto>(okResult.Value);

        Assert.Equal(expectedGameDto.Id, returnedGame.Id);
        Assert.Equal(expectedGameDto.Name, returnedGame.Name);
        Assert.Equal(expectedGameDto.Key, returnedGame.Key);
    }

    [Fact]
    public async Task GetAllGamesReturnOk()
    {
        // Arrange
        var dtoList = _expectedGameDtos;

        _mockGameService
            .Setup(s => s.GetAllGamesAsync())
            .ReturnsAsync(dtoList);

        var controller = CreateController();

        // Act
        var result = await controller.GetAllGames();

        // Assert
        _mockGameService.Verify(s => s.GetAllGamesAsync(), Times.Once);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGames = Assert.IsType<List<GameDto>>(okResult.Value);

        Assert.Equal(dtoList.Count, returnedGames.Count);
        for (int i = 0; i < dtoList.Count; i++)
        {
            Assert.Equal(dtoList[i].Id, returnedGames[i].Id);
            Assert.Equal(dtoList[i].Name, returnedGames[i].Name);
            Assert.Equal(dtoList[i].Key, returnedGames[i].Key);
        }
    }

    [Fact]
    public async Task UpdateGameReturnOk()
    {
        // Arrange
        var dto = new UpdateGameRequest();
        var controller = CreateController();

        // Act
        var result = await controller.UpdateGame(dto);

        // Assert
        _mockGameService.Verify(s => s.UpdateGameAsync(dto), Times.Once);
        var resultValue = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Game successfuly updated", resultValue.Value);
    }

    [Theory]
    [InlineData("game-key-123")]
    public async Task DeleteGameSuccessfullyDeleteReturnNoContent(string key)
    {
        // Arrange
        var controller = CreateController();
        _mockGameService
            .Setup(s => s.DeleteByKeyAsync(key))
            .ReturnsAsync(true);

        // Act
        var result = await controller.DeleteGame(key);

        // Assert
        _mockGameService.Verify(s => s.DeleteByKeyAsync(key), Times.Once);
        Assert.IsType<NoContentResult>(result);
    }

    [Theory]
    [InlineData("game-key-123")]
    public async Task DeleteGameNotFoundReturnNotFound(string key)
    {
        // Arrange
        var controller = CreateController();
        _mockGameService
            .Setup(s => s.DeleteByKeyAsync(key))
            .ReturnsAsync(false);

        // Act
        var result = await controller.DeleteGame(key);

        // Assert
        _mockGameService.Verify(s => s.DeleteByKeyAsync(key), Times.Once);
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Theory]
    [InlineData("game-key-123")]
    public async Task GetGameFileByKeyReturnOk(string key)
    {
        // Arrange
        var expectedGameDto = new GameDto
        {
            Key = key,
            Description = "Sample Game",
            Name = "Sample Game Name",
        };

        var json = JsonSerializer.Serialize(
        expectedGameDto,
        _jsonOptions);

        var fileDto = new FileDto()
        {
            Content = System.Text.Encoding.UTF8.GetBytes(json),
            FileName = expectedGameDto.Name,
        };

        _mockGameService
            .Setup(s => s.GetGameAsync(key))
            .ReturnsAsync(expectedGameDto);
        _mockGenerateGameFile.Setup(s => s.GenerateFileDto(expectedGameDto))
            .Returns(fileDto);

        var controller = CreateController();

        // Act
        var result = await controller.GetGameFileByKey(key);

        // Assert
        _mockGenerateGameFile.Verify(s => s.GenerateFileDto(expectedGameDto), Times.Once);
        var fileResult = Assert.IsType<FileContentResult>(result);

        Assert.Equal(fileDto.Content, fileResult.FileContents);
        Assert.Equal(fileDto.FileName, fileResult.FileDownloadName);
        Assert.Equal("text/plain", fileResult.ContentType);
    }

    [Theory]
    [InlineData("game-key-123")]
    public async Task GetGenreByKeyReturnOk(string key)
    {
        // Arrange
        var games = _expectedGenreDtos;

        _mockGenreService
            .Setup(s => s.GetGenresByGameKeyAsync(key))
            .ReturnsAsync(_expectedGenreDtos);

        var controller = CreateController();

        // Act
        var result = await controller.GetGenreByGameKey(key);

        // Assert
        _mockGenreService.Verify(s => s.GetGenresByGameKeyAsync(key), Times.Once);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGenres = Assert.IsType<List<GenreDto>>(okResult.Value);

        Assert.Equal(_expectedGenreDtos.Count, returnedGenres.Count);
        for (int i = 0; i < returnedGenres.Count; i++)
        {
            Assert.Equal(_expectedGenreDtos[i].Id, returnedGenres[i].Id);
            Assert.Equal(_expectedGenreDtos[i].Name, returnedGenres[i].Name);
        }
    }

    [Theory]
    [InlineData("game-key-123")]
    public async Task GetPlatoformByKeyReturnOk(string key)
    {
        // Arrange
        var games = _expectedPlatformDtos;

        _mockPlatformService
            .Setup(s => s.GetPlatformsByGameKeyAsync(key))
            .ReturnsAsync(_expectedPlatformDtos);

        var controller = CreateController();

        // Act
        var result = await controller.GetPlatformsByGameKey(key);

        // Assert
        _mockPlatformService.Verify(s => s.GetPlatformsByGameKeyAsync(key), Times.Once);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedFullGenres = Assert.IsType<List<PlatformDto>>(okResult.Value);

        Assert.Equal(_expectedPlatformDtos.Count, returnedFullGenres.Count);
        for (int i = 0; i < returnedFullGenres.Count; i++)
        {
            Assert.Equal(_expectedPlatformDtos[i].Id, returnedFullGenres[i].Id);
            Assert.Equal(_expectedPlatformDtos[i].Type, returnedFullGenres[i].Type);
        }
    }

    private GamesController CreateController()
    {
        return new GamesController(
            _mockGameService.Object,
            _mockGenreService.Object,
            _mockPlatformService.Object,
            _mockPublisherService.Object,
            _mockGenerateGameFile.Object);
    }
}