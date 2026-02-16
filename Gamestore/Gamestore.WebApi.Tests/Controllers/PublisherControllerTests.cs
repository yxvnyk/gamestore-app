using Gamestore.Application.Services.Interfaces;
using Gamestore.Domain.Models.DTO.Game;
using Gamestore.Domain.Models.DTO.Publisher;
using Gamestore.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Gamestore.WebApi.Tests.Controllers;

public class PublisherControllerTests
{
    private readonly Mock<IGameService> _mockGameService = new();
    private readonly Mock<IPublisherService> _mockPublisherService = new();

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
        }
    ];

    private readonly List<PublisherDto> _expectedPublishersDtos =
    [
        new PublisherDto
        {
            Id = Guid.NewGuid(),
            CompanyName = "Action",
        },
        new PublisherDto
        {
            Id = Guid.NewGuid(),
            CompanyName = "Adventure",
        },
        new PublisherDto
        {
            Id = Guid.NewGuid(),
            CompanyName = "RPG",
        }
    ];

    [Fact]
    public async Task CreatePublisherReturnOk()
    {
        // Arrange
        var dto = new CreatePublisherRequest();
        var controller = CreateController();

        // Act
        var result = await controller.CreatePublisher(dto);

        // Assert
        _mockPublisherService.Verify(s => s.CreatePublisherAsync(dto.Publisher), Times.Once);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task GetPublisherByCompanyNameReturnOk()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expectedGenreDto = new PublisherDto
        {
            Id = id,
            CompanyName = "Company Name",
        };

        _mockPublisherService
            .Setup(s => s.GetPublisherByCompanyNameAsync(expectedGenreDto.CompanyName))
            .ReturnsAsync(expectedGenreDto);

        var controller = CreateController();

        // Act
        var result = await controller.GetPublisherByCompanyName(expectedGenreDto.CompanyName);

        // Assert
        _mockPublisherService.Verify(s => s.GetPublisherByCompanyNameAsync(expectedGenreDto.CompanyName), Times.Once);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGenre = Assert.IsType<PublisherDto>(okResult.Value);

        Assert.Equal(expectedGenreDto.Id, returnedGenre.Id);
        Assert.Equal(expectedGenreDto.CompanyName, returnedGenre.CompanyName);
    }

    [Fact]
    public async Task GetAllGenresReturnOk()
    {
        // Arrange
        var dtoList = _expectedPublishersDtos;

        _mockPublisherService
            .Setup(s => s.GetAllPublishersAsync())
            .ReturnsAsync(dtoList);

        var controller = CreateController();

        // Act
        var result = await controller.GetAllPublishers();

        // Assert
        _mockPublisherService.Verify(s => s.GetAllPublishersAsync(), Times.Once);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGenres = Assert.IsType<List<PublisherDto>>(okResult.Value);

        Assert.Equal(dtoList.Count, returnedGenres.Count);
        for (int i = 0; i < dtoList.Count; i++)
        {
            Assert.Equal(dtoList[i].Id, returnedGenres[i].Id);
            Assert.Equal(dtoList[i].CompanyName, returnedGenres[i].CompanyName);
        }
    }

    [Fact]
    public async Task UpdateGenresReturnOk()
    {
        // Arrange
        var dto = new UpdatePublisherRequest();
        var controller = CreateController();

        // Act
        var result = await controller.UpdatePublisher(dto);

        // Assert
        _mockPublisherService.Verify(s => s.UpdatePublisherAsync(dto.Publisher), Times.Once);
        var resultValue = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("{ message = Publisher successfuly updated }", resultValue.Value.ToString());
    }

    [Fact]
    public async Task DeletePublisherSuccessfullyDeleteReturnNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var controller = CreateController();
        _mockPublisherService
            .Setup(s => s.DeletePublisherAsync(id))
            .ReturnsAsync(true);

        // Act
        var result = await controller.DeletePublisher(id);

        // Assert
        _mockPublisherService.Verify(s => s.DeletePublisherAsync(id), Times.Once);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteGenreNotFoundReturnNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var controller = CreateController();
        _mockPublisherService
            .Setup(s => s.DeletePublisherAsync(id))
            .ReturnsAsync(false);

        // Act
        var result = await controller.DeletePublisher(id);

        // Assert
        _mockPublisherService.Verify(s => s.DeletePublisherAsync(id), Times.Once);
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetGamesByPublisherNameReturnOk()
    {
        // Arrange
        var expectedGames = _expectedGameDtos;
        var id = Guid.NewGuid();
        var companyName = "Sample Company Name";
        var controller = CreateController();
        _mockGameService
            .Setup(s => s.GetGamesByCompanyNameAsync(companyName))
            .ReturnsAsync(expectedGames);

        // Act
        var result = await controller.GetGamesByPublisherName(companyName);

        // Assert
        _mockGameService.Verify(s => s.GetGamesByCompanyNameAsync(companyName), Times.Once);
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

    private PublishersController CreateController()
    {
        return new PublishersController(
            _mockPublisherService.Object,
            _mockGameService.Object);
    }
}