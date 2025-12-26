using Gamestore.Application.Services.Interfaces;
using Gamestore.Domain.Models.DTO;
using Gamestore.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Gamestore.WebApi.Tests.Controllers;

public class GenreControllerTests
{
    private readonly Mock<IGameService> _mockGameService = new();
    private readonly Mock<IGenreService> _mockGenreService = new();

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

    private readonly List<GenreDto> _genreDtosWithSameParentId =
    [
         new GenreDto
         {
             Name = "Sample Genre Name",
             Id = Guid.Empty,
         },
         new GenreDto
         {
             Name = "Sample Genre Name",
             Id = Guid.Empty,
         },
         new GenreDto
         {
             Name = "Sample Genre Name",
             Id = Guid.Empty,
         },
    ];

    [Fact]
    public async Task CreateGenreReturnOk()
    {
        var dto = new GenreCreateDto();

        var controller = CreateController();

        // Act
        var result = await controller.CreateGenre(dto);

        // Assert
        _mockGenreService.Verify(s => s.CreateGenreAsync(dto), Times.Once);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task GetGenreByIdReturnOk()
    {
        var id = Guid.NewGuid();
        var expectedGenreDto = new GenreFullDto
        {
            Name = "Sample Genre Name",
            Id = id,
            ParentGenreId = null,
        };

        _mockGenreService
            .Setup(s => s.GetGenreByIdAsync(id))
            .ReturnsAsync(expectedGenreDto);

        var controller = CreateController();

        // Act
        var result = await controller.GetGenreById(id);

        // Assert
        _mockGenreService.Verify(s => s.GetGenreByIdAsync(id), Times.Once);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGenre = Assert.IsType<GenreFullDto>(okResult.Value);

        Assert.Equal(expectedGenreDto.Id, returnedGenre.Id);
        Assert.Equal(expectedGenreDto.Name, returnedGenre.Name);
    }

    [Fact]
    public async Task GetGenreByParentIdReturnOk()
    {
        var id = Guid.NewGuid();
        var expectedGenreDtos = _genreDtosWithSameParentId;

        _mockGenreService
            .Setup(s => s.GetGenresByParentIdAsync(id))
            .ReturnsAsync(expectedGenreDtos);

        var controller = CreateController();

        // Act
        var result = await controller.GetGenreByParentId(id);

        // Assert
        _mockGenreService.Verify(s => s.GetGenresByParentIdAsync(id), Times.Once);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGenres = Assert.IsAssignableFrom<List<GenreDto>>(okResult.Value);

        Assert.Equal(expectedGenreDtos.Count, returnedGenres.Count);
        for (int i = 0; i < expectedGenreDtos.Count; i++)
        {
            Assert.Equal(expectedGenreDtos[i].Id, returnedGenres[i].Id);
            Assert.Equal(expectedGenreDtos[i].Name, returnedGenres[i].Name);
        }
    }

    [Fact]
    public async Task GetAllGenresReturnOk()
    {
        var dtoList = _expectedGenreDtos;

        _mockGenreService
            .Setup(s => s.GetAllGenresAsync())
            .ReturnsAsync(dtoList);

        var controller = CreateController();

        // Act
        var result = await controller.GetAllGenres();

        // Assert
        _mockGenreService.Verify(s => s.GetAllGenresAsync(), Times.Once);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGenres = Assert.IsType<List<GenreDto>>(okResult.Value);

        Assert.Equal(dtoList.Count, returnedGenres.Count);
        for (int i = 0; i < dtoList.Count; i++)
        {
            Assert.Equal(dtoList[i].Id, returnedGenres[i].Id);
            Assert.Equal(dtoList[i].Name, returnedGenres[i].Name);
        }
    }

    [Fact]
    public async Task UpdateGenresReturnOk()
    {
        var dto = new GenreUpdateDto();

        var controller = CreateController();

        // Act
        var result = await controller.UpdateGenre(dto);

        // Assert
        _mockGenreService.Verify(s => s.UpdateGenreAsync(dto), Times.Once);
        var resultValue = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Genre successfuly updated", resultValue.Value);
    }

    [Fact]
    public async Task DeleteGenreSuccessfullyDeleteReturnNoContent()
    {
        var id = Guid.NewGuid();
        var controller = CreateController();
        _mockGenreService
            .Setup(s => s.DeleteByIdAsync(id))
            .ReturnsAsync(true);

        // Act
        var result = await controller.DeleteGenre(id);

        // Assert
        _mockGenreService.Verify(s => s.DeleteByIdAsync(id), Times.Once);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteGenreNotFoundReturnNotFound()
    {
        var id = Guid.NewGuid();
        var controller = CreateController();
        _mockGenreService
            .Setup(s => s.DeleteByIdAsync(id))
            .ReturnsAsync(false);

        // Act
        var result = await controller.DeleteGenre(id);

        // Assert
        _mockGenreService.Verify(s => s.DeleteByIdAsync(id), Times.Once);
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetGamesByGenreReturnOk()
    {
        var expectedGames = _expectedGameDtos;
        var id = Guid.NewGuid();
        var controller = CreateController();
        _mockGameService
            .Setup(s => s.GetGamesByGenreAsync(id))
            .ReturnsAsync(expectedGames);

        // Act
        var result = await controller.GetGamesByGenre(id);

        // Assert
        _mockGameService.Verify(s => s.GetGamesByGenreAsync(id), Times.Once);
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

    private GenresController CreateController()
    {
        return new GenresController(
            _mockGenreService.Object,
            _mockGameService.Object);
    }
}