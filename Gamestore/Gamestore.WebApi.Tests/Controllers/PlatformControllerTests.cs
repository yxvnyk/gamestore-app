using Gamestore.Application.Services.Interfaces;
using Gamestore.Domain.Models.DTO.Game;
using Gamestore.Domain.Models.DTO.Platform;
using Gamestore.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Gamestore.WebApi.Tests.Controllers;

public class PlatformControllerTests
{
    private readonly Mock<IGameService> _mockGameService = new();
    private readonly Mock<IPlatformService> _mockPlatformService = new();

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

    private readonly List<PlatformDto> _expectedPlatformDtos =
    [
        new PlatformDto
        {
            Id = Guid.NewGuid(),
            Type = "IOS",
        },
        new PlatformDto
        {
            Id = Guid.NewGuid(),
            Type = "Android",
        },
        new PlatformDto
        {
            Id = Guid.NewGuid(),
            Type = "Unix",
        }
    ];

    [Fact]
    public async Task CreateGenreReturnOk()
    {
        // Arrange
        var dto = new CreatePlatformRequest();
        var controller = CreateController();

        // Act
        var result = await controller.CreatePlatform(dto);

        // Assert
        _mockPlatformService.Verify(s => s.CreatePlatformAsync(dto.Platform), Times.Once);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task GetPlatformByIdReturnOk()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expectedPlatformDto = new PlatformDto
        {
            Type = "Sample Platform Name",
            Id = id,
        };

        _mockPlatformService
            .Setup(s => s.GetPlatformByIdAsync(id))
            .ReturnsAsync(expectedPlatformDto);

        var controller = CreateController();

        // Act
        var result = await controller.GetPlatformById(id);

        // Assert
        _mockPlatformService.Verify(s => s.GetPlatformByIdAsync(id), Times.Once);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGenre = Assert.IsType<PlatformDto>(okResult.Value);

        Assert.Equal(expectedPlatformDto.Id, returnedGenre.Id);
        Assert.Equal(expectedPlatformDto.Type, returnedGenre.Type);
    }

    [Fact]
    public async Task GetAllPlatformsReturnOk()
    {
        // Arrange
        var dtoList = _expectedPlatformDtos;

        _mockPlatformService
            .Setup(s => s.GetAllPlatformsAsync())
            .ReturnsAsync(dtoList);

        var controller = CreateController();

        // Act
        var result = await controller.GetAllPlatfomrs();

        // Assert
        _mockPlatformService.Verify(s => s.GetAllPlatformsAsync(), Times.Once);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGenres = Assert.IsType<List<PlatformDto>>(okResult.Value);

        Assert.Equal(dtoList.Count, returnedGenres.Count);
        for (int i = 0; i < dtoList.Count; i++)
        {
            Assert.Equal(dtoList[i].Id, returnedGenres[i].Id);
            Assert.Equal(dtoList[i].Type, returnedGenres[i].Type);
        }
    }

    [Fact]
    public async Task UpdatePlatformsReturnOk()
    {
        // Arrange
        var dto = new UpdatePlatformRequest();
        var controller = CreateController();

        // Act
        var result = await controller.UpdatePlatform(dto);

        // Assert
        _mockPlatformService.Verify(s => s.UpdatePlatformAsync(dto.Platform), Times.Once);
        var resultValue = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("{ message = Platform successfuly updated }", resultValue.Value.ToString());
    }

    [Fact]
    public async Task DeleteGenreSuccessfullyDeleteReturnNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var controller = CreateController();
        _mockPlatformService
            .Setup(s => s.DeleteByIdAsync(id))
            .ReturnsAsync(true);

        // Act
        var result = await controller.DeletePlatform(id);

        // Assert
        _mockPlatformService.Verify(s => s.DeleteByIdAsync(id), Times.Once);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteGenreNotFoundReturnNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var controller = CreateController();
        _mockPlatformService
            .Setup(s => s.DeleteByIdAsync(id))
            .ReturnsAsync(false);

        // Act
        var result = await controller.DeletePlatform(id);

        // Assert
        _mockPlatformService.Verify(s => s.DeleteByIdAsync(id), Times.Once);
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetGamesByPlatformReturnOk()
    {
        // Arrange
        var expectedGames = _expectedGameDtos;
        var id = Guid.NewGuid();
        var controller = CreateController();
        _mockGameService
            .Setup(s => s.GetGamesByPlatformAsync(id))
            .ReturnsAsync(expectedGames);

        // Act
        var result = await controller.GetGamesByPlatform(id);

        // Assert
        _mockGameService.Verify(s => s.GetGamesByPlatformAsync(id), Times.Once);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGames = Assert.IsType<List<GameDto>>(okResult.Value);

        Assert.Equal(expectedGames.Count, returnedGames.Count);
        for (int i = 0; i < expectedGames.Count; i++)
        {
            Assert.Equal(expectedGames[i].Id, returnedGames[i].Id);
            Assert.Equal(expectedGames[i].Name, returnedGames[i].Name);
            Assert.Equal(expectedGames[i].Key, returnedGames[i].Key);
        }
    }

    private PlatformsController CreateController()
    {
        return new PlatformsController(
            _mockPlatformService.Object,
            _mockGameService.Object);
    }
}