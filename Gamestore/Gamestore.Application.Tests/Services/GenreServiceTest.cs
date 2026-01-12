using AutoMapper;
using Gamestore.Application.Services;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models.DTO;
using Moq;

namespace Gamestore.Application.Tests.Services;

public class GenreServiceTest
{
    private readonly Mock<IGameRepository> _mockGameRepo = new();
    private readonly Mock<IGenreRepository> _mockGenreRepo = new();
    private readonly Mock<IMapper> _mockMapper = new();

    [Fact]
    public async Task CreateGenreAsync_ParentNotExist_ThrowsArgumentException()
    {
        // Arrange
        var parentId = Guid.NewGuid();
        var genreDto = new GenreCreateDto
        {
            Name = "Test Genre",
            ParentGenreId = parentId,
        };

        _mockGenreRepo.Setup(r => r.GenreExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        var genreService = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await genreService.CreateGenreAsync(genreDto));
        _mockGenreRepo.Verify(r => r.GenreExistsAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task CreateGenreAsync_HappyCreating_GenerateNewGenreEntity()
    {
        // Arrange
        var genreDto = new GenreCreateDto
        {
            Name = "Test Genre",
            ParentGenreId = Guid.NewGuid(),
        };

        var entity = new Genre
        {
            Name = genreDto.Name,
            ParentGenreId = genreDto.ParentGenreId,
        };

        _mockMapper.Setup(m => m.Map<Genre>(genreDto)).Returns(entity);

        _mockGenreRepo.Setup(r => r.GenreExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);
        var gameService = CreateService();

        // Act
        await gameService.CreateGenreAsync(genreDto);

        // Assert
        _mockMapper.Verify(m => m.Map<GenreCreateDto, Genre>(genreDto), Times.Once);
        _mockGenreRepo.Verify(r => r.GenreExistsAsync(genreDto.ParentGenreId.Value), Times.Once);
        _mockGenreRepo.Verify(r => r.CreateGenreAsync(It.IsAny<Genre>()), Times.Once);
    }

    [Fact]
    public async Task GetAllGenresAsync_WhenGenresExists_ReturnGenreDTOs()
    {
        // Arrange
        var genre1 = CreateGenre("Genre 1");
        var genre2 = CreateGenre("Genre 2");
        var genreDto1 = CreateGenreDto(genre1);
        var genreDto2 = CreateGenreDto(genre2);

        _mockGenreRepo.Setup(r => r.GetAllGenresAsync()).ReturnsAsync(
        [
            genre1,
            genre2,
        ]);
        _mockMapper.Setup(m => m.Map<GenreDto>(genre1)).Returns(genreDto1);
        _mockMapper.Setup(m => m.Map<GenreDto>(genre2)).Returns(genreDto2);

        var service = CreateService();

        // Act
        var result = (await service.GetAllGenresAsync()).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(genreDto1, result[0]);
        Assert.Equal(genreDto2, result[1]);
        _mockMapper.Verify(m => m.Map<GenreDto>(It.IsAny<Genre>()), Times.Exactly(2));
        _mockGenreRepo.Verify(r => r.GetAllGenresAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllGenresAsync_NoGenres_ReturnEmptyCollection()
    {
        // Arrange
        _mockGenreRepo.Setup(r => r.GetAllGenresAsync())
            .ReturnsAsync([]);

        var service = CreateService();

        // Act
        var result = await service.GetAllGenresAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _mockGenreRepo.Verify(r => r.GetAllGenresAsync(), Times.Once);
        _mockMapper.Verify(m => m.Map<GenreDto>(It.IsAny<Genre>()), Times.Never);
    }

    [Fact]
    public async Task GetAllGenreseByGameKeyAsync_NotFound_ThrowException()
    {
        // Arrange
        string genreKey = "test-game-key";
        _mockGameRepo.Setup(r => r.GameKeyExistAsync(genreKey)).ReturnsAsync(false);

        var service = CreateService();

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(async () => await service.GetGenresByGameKeyAsync(genreKey));

        // Assert
        Assert.Contains(genreKey, ex.Message);
        _mockGameRepo.Verify(r => r.GameKeyExistAsync(genreKey), Times.Once);
        _mockMapper.Verify(m => m.Map<GenreDto>(It.IsAny<Genre>()), Times.Never);
    }

    [Fact]
    public async Task GetGenreseByGameKeyAsync_GenresFound_ReturnGenresDTOsCollection()
    {
        // Arrange
        string genreKey = "test-game-key";

        var genre1 = CreateGenre("Genre 1");
        var genre2 = CreateGenre("Genre 2");
        var genreDto1 = CreateGenreDto(genre1);
        var genreDto2 = CreateGenreDto(genre2);

        _mockGenreRepo.Setup(r => r.GetGenresByGameKeyAsync(genreKey)).ReturnsAsync(
        [
            genre1,
            genre2,
        ]);
        _mockMapper.Setup(m => m.Map<GenreDto>(genre1)).Returns(genreDto1);
        _mockMapper.Setup(m => m.Map<GenreDto>(genre2)).Returns(genreDto2);
        _mockGameRepo.Setup(r => r.GameKeyExistAsync(genreKey)).ReturnsAsync(true);

        var service = CreateService();

        // Act
        var resutl = await service.GetGenresByGameKeyAsync(genreKey);

        // Assert
        _mockGameRepo.Verify(r => r.GameKeyExistAsync(genreKey), Times.Once);
        _mockGenreRepo.Verify(r => r.GetGenresByGameKeyAsync(genreKey), Times.Once);
        _mockMapper.Verify(r => r.Map<GenreDto>(It.IsAny<Genre>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetGenreByIdAsync_GenreFound_ReturnGenresCollection()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var genreDto = new GenreFullDto
        {
            Id = genreId,
            Name = "Genre 1",
            ParentGenreId = null,
        };
        var genre = CreateGenre();

        _mockGenreRepo.Setup(repo => repo.GetGenreByIdAsync(genreId))!.ReturnsAsync(genre);
        _mockMapper.Setup(mockMapper => mockMapper.Map<GenreFullDto>(genre)).Returns(genreDto);

        var service = CreateService();

        // Act
        var result = await service.GetGenreByIdAsync(genreId);

        // Assert
        _mockGenreRepo.Verify(r => r.GetGenreByIdAsync(genreId), Times.Once);
        _mockMapper.Verify(m => m.Map<GenreFullDto>(It.IsAny<Genre>()), Times.Once);
    }

    [Fact]
    public async Task GetGenreByIdAsync_NotFound_ThrowsNotFoundException()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        _mockGenreRepo.Setup(repo => repo.GetGenreByIdAsync(genreId))!.ReturnsAsync((Genre?)null);

        var service = CreateService();

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(async () => await service.GetGenreByIdAsync(genreId));

        // Assert
        Assert.Contains(genreId.ToString(), ex.Message);
        _mockMapper.Verify(m => m.Map<GenreFullDto>(It.IsAny<Genre>()), Times.Never);
    }

    [Fact]
    public async Task GetGenresByParentIdAsync_NotFound_ThrowsNotFoundException()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        _mockGenreRepo.Setup(repo => repo.GenreExistsAsync(genreId))!.ReturnsAsync(false);

        var service = CreateService();

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(async () => await service.GetGenresByParentIdAsync(genreId));

        // Assert
        Assert.Contains(genreId.ToString(), ex.Message);
        _mockGenreRepo.Verify(r => r.GetGenreByIdAsync(genreId), Times.Never);
    }

    [Fact]
    public async Task GetGenresByParentIdAsync_GenresFound_ReturnGenresDTOsCollection()
    {
        // Arrange
        var parentGenreId = Guid.NewGuid();

        var genre1 = CreateGenre("Genre 1");
        var genre2 = CreateGenre("Genre 2");
        var genreDto1 = CreateGenreDto(genre1);
        var genreDto2 = CreateGenreDto(genre2);

        _mockGenreRepo.Setup(r => r.GetGenresByParentIdAsync(parentGenreId)).ReturnsAsync(
        [
            genre1,
            genre2,
        ]);
        _mockMapper.Setup(m => m.Map<GenreDto>(genre1)).Returns(genreDto1);
        _mockMapper.Setup(m => m.Map<GenreDto>(genre2)).Returns(genreDto2);
        _mockGenreRepo.Setup(r => r.GenreExistsAsync(parentGenreId)).ReturnsAsync(true);

        var service = CreateService();

        // Act
        var resutl = await service.GetGenresByParentIdAsync(parentGenreId);

        // Assert
        _mockGenreRepo.Verify(r => r.GetGenresByParentIdAsync(parentGenreId), Times.Once);
        _mockGenreRepo.Verify(r => r.GenreExistsAsync(parentGenreId), Times.Once);
        _mockMapper.Verify(r => r.Map<GenreDto>(It.IsAny<Genre>()), Times.Exactly(2));
    }

    [Fact]
    public async Task UpdateGenreAsync_WhenGenreExists_UpdatesSuccessfully()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var parent = Guid.NewGuid();
        var name = "Test Genre";
        var genreDto = new GenreUpdateDto
        {
            Id = gameId,
            Name = name,
            ParentGenreId = parent,
        };
        var genre = CreateGenre(name, parent);
        genre.Id = gameId;

        _mockGenreRepo.Setup(r => r.GetGenreByIdAsync(gameId)).ReturnsAsync(genre);
        _mockMapper.Setup(m => m.Map(genreDto, genre)).Returns(genre);
        _mockGenreRepo.Setup(r => r.UpdateGenreAsync(genre)).Returns(Task.CompletedTask);

        var genreService = CreateService();

        // Act
        await genreService.UpdateGenreAsync(genreDto);

        // Assert
        _mockMapper.Verify(m => m.Map(genreDto, genre), Times.Once);
        _mockGenreRepo.Verify(r => r.GetGenreByIdAsync(gameId), Times.Once);
        _mockGenreRepo.Verify(r => r.UpdateGenreAsync(genre), Times.Once);
    }

    [Fact]
    public async Task UpdateGenreAsync_GenreNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var parent = Guid.NewGuid();
        var name = "Test Genre";
        var genreDto = new GenreUpdateDto
        {
            Id = genreId,
            Name = name,
            ParentGenreId = parent,
        };
        var genre = CreateGenre(name, parent);

        _mockGenreRepo.Setup(r => r.GetGenreByIdAsync(genreId))!.ReturnsAsync((Genre?)null);

        var genreService = CreateService();

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(async () => await genreService.UpdateGenreAsync(genreDto));

        // Assert
        Assert.Contains(genreId.ToString(), ex.Message);
        _mockGenreRepo.Verify(r => r.GetGenreByIdAsync(genreId), Times.Once);
    }

    [Fact]
    public async Task DeleteByIdAsync_HappyDelete_ReturnTrue()
    {
        // Arrange
        _mockGenreRepo.Setup(r => r.DeleteByIdAsync(It.IsAny<Guid>())).ReturnsAsync(true);

        var service = CreateService();

        // Act
        var result = await service.DeleteByIdAsync(Guid.NewGuid());

        // Assert
        Assert.True(result);
        _mockGenreRepo.Verify(r => r.DeleteByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    private static Genre CreateGenre(
    string name = "Genre",
    Guid? parentGenreId = null)
    => new()
    {
        Id = Guid.NewGuid(),
        ParentGenreId = parentGenreId,
        Name = name,
    };

    private static GenreDto CreateGenreDto(Genre genre)
        => new()
        {
            Id = genre.Id,
            Name = genre.Name,
        };

    private GenreService
 CreateService() => new(
      _mockGenreRepo.Object,
      _mockGameRepo.Object,
      _mockMapper.Object);
}