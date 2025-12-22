using AutoMapper;
using Gamestore.Application.Exceptions;
using Gamestore.Application.Services;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Models.DTO;
using Moq;

namespace Gamestore.Application.UnitTests.Services;

public class PlatformServiceTest
{
    private readonly Mock<IGameRepository> _mockGameRepo = new();
    private readonly Mock<IPlatformRepository> _mockPlatformRepo = new();
    private readonly Mock<IMapper> _mockMapper = new();

    [Fact]
    public async Task CreatePlatformAsync_HappyCreating_GenerateNewPlatformEntity()
    {
        var platformDto = new PlatformDto
        {
            Type = "Test Platform",
        };

        var entity = new Platform
        {
            Type = platformDto.Type,
        };

        _mockMapper.Setup(m => m.Map<Platform>(platformDto)).Returns(entity);
        var platformService = CreateService();

        await platformService.CreatePlatformAsync(platformDto);

        _mockMapper.Verify(m => m.Map<PlatformDto, Platform>(platformDto), Times.Once);

        _mockPlatformRepo.Verify(
            r => r.CreatePlatformAsync(It.IsAny<Platform>()),
            Times.Once);
    }

    [Fact]
    public async Task GetAllPlatformsAsync_WhenPlatformExists_ReturnPlatformDTOs()
    {
        var platform1 = CreatePlatform("Platform 1");
        var platform2 = CreatePlatform("Platform 2");
        var platformDto1 = CreatePlatformFullDto(platform1);
        var platformDto2 = CreatePlatformFullDto(platform2);

        _mockPlatformRepo.Setup(r => r.GetAllPlatformsAsync()).ReturnsAsync(
        [
            platform1,
            platform2,
        ]);
        _mockMapper.Setup(m => m.Map<PlatformFullDto>(platform1)).Returns(platformDto1);
        _mockMapper.Setup(m => m.Map<PlatformFullDto>(platform2)).Returns(platformDto2);

        var service = CreateService();

        var result = (await service.GetAllPlatformsAsync()).ToList();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(platformDto1, result[0]);
        Assert.Equal(platformDto2, result[1]);

        _mockMapper.Verify(m => m.Map<PlatformFullDto>(It.IsAny<Platform>()), Times.Exactly(2));
        _mockPlatformRepo.Verify(r => r.GetAllPlatformsAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllPlatformAsync_NoPlatform_ReturnEmptyCollection()
    {
        // Arrange
        _mockPlatformRepo.Setup(r => r.GetAllPlatformsAsync())
            .ReturnsAsync([]);

        var service = CreateService();

        // Act
        var result = await service.GetAllPlatformsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _mockPlatformRepo.Verify(r => r.GetAllPlatformsAsync(), Times.Once);
        _mockMapper.Verify(m => m.Map<PlatformFullDto>(It.IsAny<Platform>()), Times.Never);
    }

    [Fact]
    public async Task GetAllPlatformByGameKeyAsync_NotFound_ThrowException()
    {
        string latformKey = "test-game-key";
        _mockGameRepo.Setup(r => r.GameKeyExistAsync(latformKey)).ReturnsAsync(false);

        var service = CreateService();

        var ex = await Assert.ThrowsAsync<NotFoundException>(async () => await service.GetPlatformsByGameKeyAsync(latformKey));
        Assert.Contains(latformKey, ex.Message);
        _mockGameRepo.Verify(r => r.GameKeyExistAsync(latformKey), Times.Once);
        _mockMapper.Verify(m => m.Map<PlatformFullDto>(It.IsAny<Platform>()), Times.Never);
    }

    [Fact]
    public async Task GetPlatformByGameKeyAsync_PlatformFound_ReturnPlatformDTOsCollection()
    {
        string genreKey = "test-game-key";

        var platform1 = CreatePlatform("Genre 1");
        var platform2 = CreatePlatform("Genre 2");
        var platformDto1 = CreatePlatformFullDto(platform1);
        var plaformDto2 = CreatePlatformFullDto(platform2);

        _mockPlatformRepo.Setup(r => r.GetPlatformsByGameKeyAsync(genreKey)).ReturnsAsync(
        [
            platform1,
            platform2,
        ]);
        _mockMapper.Setup(m => m.Map<PlatformFullDto>(platform1)).Returns(platformDto1);
        _mockMapper.Setup(m => m.Map<PlatformFullDto>(platform2)).Returns(plaformDto2);
        _mockGameRepo.Setup(r => r.GameKeyExistAsync(genreKey)).ReturnsAsync(true);

        var service = CreateService();

        var resutl = await service.GetPlatformsByGameKeyAsync(genreKey);
        _mockGameRepo.Verify(r => r.GameKeyExistAsync(genreKey), Times.Once);
        _mockPlatformRepo.Verify(r => r.GetPlatformsByGameKeyAsync(genreKey), Times.Once);
        _mockMapper.Verify(r => r.Map<PlatformFullDto>(It.IsAny<Platform>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetPlatformByIdAsync_PlatformFound_ReturnPlatformsCollection()
    {
        var latformId = Guid.NewGuid();
        var platformDto = new PlatformFullDto
        {
            Type = "Platform 1",
        };
        var genre = CreatePlatform();

        _mockPlatformRepo.Setup(repo => repo.GetPlatformByIdAsync(latformId))!.ReturnsAsync(genre);
        _mockMapper.Setup(mockMapper => mockMapper.Map<PlatformFullDto>(genre)).Returns(platformDto);

        var service = CreateService();

        var result = await service.GetPlatformByIdAsync(latformId);
        _mockPlatformRepo.Verify(r => r.GetPlatformByIdAsync(latformId), Times.Once);
        _mockMapper.Verify(m => m.Map<PlatformFullDto>(It.IsAny<Platform>()), Times.Once);
    }

    [Fact]
    public async Task GetPlatformByIdAsync_NotFound_ThrowsNotFoundException()
    {
        var platformId = Guid.NewGuid();
        _mockPlatformRepo.Setup(repo => repo.GetPlatformByIdAsync(platformId))!.ReturnsAsync((Platform?)null);

        var service = CreateService();

        var ex = await Assert.ThrowsAsync<NotFoundException>(async () => await service.GetPlatformByIdAsync(platformId));
        Assert.Contains(platformId.ToString(), ex.Message);
        _mockMapper.Verify(m => m.Map<PlatformFullDto>(It.IsAny<Platform>()), Times.Never);
    }

    [Fact]
    public async Task UpdatePlatformAsync_WhenPlatformExists_UpdatesSuccessfully()
    {
        var platformId = Guid.NewGuid();
        var type = "Test Platform";
        var platformDto = new PlatformUpdateDto
        {
            Type = type,
            Id = platformId,
        };
        var genre = CreatePlatform(type);
        genre.Id = platformId;

        _mockPlatformRepo.Setup(r => r.GetPlatformByIdAsync(platformId)).ReturnsAsync(genre);
        _mockMapper.Setup(m => m.Map(platformDto, genre)).Returns(genre);
        _mockPlatformRepo.Setup(r => r.UpdatePlatformAsync(genre)).Returns(Task.CompletedTask);

        var genreService = CreateService();

        await genreService.UpdatePlatformAsync(platformDto);

        _mockMapper.Verify(m => m.Map(platformDto, genre), Times.Once);
        _mockPlatformRepo.Verify(r => r.GetPlatformByIdAsync(platformId), Times.Once);
        _mockPlatformRepo.Verify(r => r.UpdatePlatformAsync(genre), Times.Once);
    }

    [Fact]
    public async Task UpdatePlatformAsync_PlatformNotExist_ThrowsNotFoundException()
    {
        var platformId = Guid.NewGuid();
        var type = "Test Platform";
        var platformDto = new PlatformUpdateDto
        {
            Type = type,
            Id = platformId,
        };
        var genre = CreatePlatform(type);
        genre.Id = platformId;

        _mockPlatformRepo.Setup(r => r.GetPlatformByIdAsync(platformId))!.ReturnsAsync((Platform?)null);

        var service = CreateService();

        var ex = await Assert.ThrowsAsync<NotFoundException>(async () => await service.UpdatePlatformAsync(platformDto));
        Assert.Contains(platformId.ToString(), ex.Message);
        _mockPlatformRepo.Verify(r => r.GetPlatformByIdAsync(platformId), Times.Once);
    }

    [Fact]
    public async Task DeleteByIdAsync_HappyDelete_ReturnTrue()
    {
        _mockPlatformRepo.Setup(r => r.DeleteByIdAsync(It.IsAny<Guid>())).ReturnsAsync(true);

        var service = CreateService();

        var result = await service.DeleteByIdAsync(Guid.NewGuid());
        Assert.True(result);
        _mockPlatformRepo.Verify(r => r.DeleteByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    private PlatformService CreateService() => new(
    _mockPlatformRepo.Object,
    _mockGameRepo.Object,
    _mockMapper.Object);

    private static Platform CreatePlatform(
    string type = "Platform")

    => new()
    {
        Id = Guid.NewGuid(),
        Type = type,
    };

    private static PlatformFullDto CreatePlatformFullDto(Platform platform)
        => new()
        {
            Type = platform.Type,
        };
}