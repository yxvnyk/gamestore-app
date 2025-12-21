using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Models.DTO;
using Gamestore.WebApi.Controllers;
using GameStore.Application.Helpers.Interfaces;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Gamestore.WebApi.UnitTests
{
    public class GameControllerTests
    {
        private readonly Mock<IGameService> mockGameService = new();
        private readonly Mock<IGenreService> mockGenreService = new();
        private readonly Mock<IPlatformService> mockPlatformService = new();
        private readonly Mock<IGenerateGameFile> mockGenerateGameFile = new();
        private Guid guid = Guid.NewGuid();

        private GamesController CreateController()
        {
            return new GamesController(
                mockGameService.Object, 
                mockGenreService.Object, 
                mockPlatformService.Object,
                mockGenerateGameFile.Object);
        }

        private readonly List<GameDto> expectedGameDtos = new()
        {
            new GameDto
            {
                Id = Guid.NewGuid(),
                Key = "game-key-123",
                Name = "Sample Game 1",
                Description = "Description for Sample Game 1"
            },
            new GameDto
            {
                Id = Guid.NewGuid(),
                Key = "game-key-456",
                Name = "Sample Game 2",
                Description = "Description for Sample Game 2"
            },
            new GameDto
            {
                Id = Guid.NewGuid(),
                Key = "game-key-789",
                Name = "Sample Game 3",
                Description = "Description for Sample Game 3"
            }
        };
        private readonly List<GenreDto> expectedGenreDtos = new()
        {
            new GenreDto
            {
                Id = Guid.NewGuid(),
                Name = "Action"
            },
            new GenreDto
            {
                Id = Guid.NewGuid(),
                Name = "Adventure"
            },
            new GenreDto
            {
                Id = Guid.NewGuid(),
                Name = "RPG"
            }
        };

        private readonly List<PlatformFullDto> expectedPlatformDtos = new()
        {
            new PlatformFullDto
            {
                Id = Guid.NewGuid(),
                Type = "PC"
            },
            new PlatformFullDto
            {
                Id = Guid.NewGuid(),
                Type = "Console"
            },
            new PlatformFullDto
            {
                Id = Guid.NewGuid(),
                Type = "Mobile"
            }
        };

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true,
        };

        [Fact]
        public async Task CreateGame_ReturnOk()
        {
            var dto = new GameCreateExtendedDto();

            var controller = CreateController();

            // Act
            var result = await controller.CreateGame(dto);

            // Assert
            mockGameService.Verify(s => s.CreateGameAsync(dto), Times.Once);
            Assert.IsType<OkResult>(result);
        }

        [Theory]
        [InlineData("game-key-123")]
        public async Task GetGameByKey_ReturnOk(string key)
        {
            var expectedGameDto = new GameDto { 
                Key = key,
                Description = "Sample Game",
                Name = "Sample Game Name"
            };

            mockGameService
                .Setup(s => s.GetGameAsync(key))
                .ReturnsAsync(expectedGameDto);

            var controller = CreateController();

            // Act
            var result = await controller.GetGameByKey(key);

            // Assert
            mockGameService.Verify(s => s.GetGameAsync(key), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedGame = Assert.IsType<GameDto>(okResult.Value);

            Assert.Equal(expectedGameDto.Id, returnedGame.Id);
            Assert.Equal(expectedGameDto.Name, returnedGame.Name);
            Assert.Equal(expectedGameDto.Key, returnedGame.Key);
        }

        [Fact]
        public async Task GetGameById_ReturnOk()
        {
            var id = Guid.NewGuid();
            var expectedGameDto = new GameDto { 
                Key = "key",
                Description = "Sample Game",
                Name = "Sample Game Name"
            };

            mockGameService
                .Setup(s => s.GetGameAsync(id))
                .ReturnsAsync(expectedGameDto);

            var controller = CreateController();

            // Act
            var result = await controller.GetGameById(id);

            // Assert
            mockGameService.Verify(s => s.GetGameAsync(id), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedGame = Assert.IsType<GameDto>(okResult.Value);

            Assert.Equal(expectedGameDto.Id, returnedGame.Id);
            Assert.Equal(expectedGameDto.Name, returnedGame.Name);
            Assert.Equal(expectedGameDto.Key, returnedGame.Key);
        }

        [Fact]
        public async Task GetAllGames_ReturnOk()
        {
            var dtoList = expectedGameDtos;

            mockGameService
                .Setup(s => s.GetAllGamesAsync())
                .ReturnsAsync(dtoList);

            var controller = CreateController();

            // Act
            var result = await controller.GetAllGames();

            // Assert
            mockGameService.Verify(s => s.GetAllGamesAsync(), Times.Once);
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
        public async Task UpdateGame_ReturnOk()
        {
            var dto = new GameUpdateExtendedDto();

            var controller = CreateController();

            // Act
            var result = await controller.UpdateGame(dto);

            // Assert
            mockGameService.Verify(s => s.UpdateGameAsync(dto), Times.Once);
            var resultValue = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Game successfuly updated", resultValue.Value);
        }

        [Theory]
        [InlineData("game-key-123")]
        public async Task DeleteGame_SuccessfullyDelete_ReturnNoContent(string key)
        {
            var controller = CreateController();
            mockGameService
                .Setup(s => s.DeleteByKeyAsync(key))
                .ReturnsAsync(true);

            // Act
            var result = await controller.DeleteGame(key);

            // Assert
            mockGameService.Verify(s => s.DeleteByKeyAsync(key), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }

        [Theory]
        [InlineData("game-key-123")]
        public async Task DeleteGame_NotFound_ReturnNotFound(string key)
        {
            var controller = CreateController();
            mockGameService
                .Setup(s => s.DeleteByKeyAsync(key))
                .ReturnsAsync(false);

            // Act
            var result = await controller.DeleteGame(key);

            // Assert
            mockGameService.Verify(s => s.DeleteByKeyAsync(key), Times.Once);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Theory]
        [InlineData("game-key-123")]
        public async Task GetGameFileByKey_ReturnOk(string key)
        {
            var expectedGameDto = new GameDto
            {
                Key = key,
                Description = "Sample Game",
                Name = "Sample Game Name"
            };

            var json = JsonSerializer.Serialize(
            expectedGameDto,
            _jsonOptions);

            var fileDto = new FileDto()
            {
                Content = System.Text.Encoding.UTF8.GetBytes(json),
                FileName = expectedGameDto.Name,
            };

            mockGameService
                .Setup(s => s.GetGameAsync(key))
                .ReturnsAsync(expectedGameDto);
            mockGenerateGameFile.Setup(s => s.GenerateFileDto(expectedGameDto))
                .Returns(fileDto);

            var controller = CreateController();

            // Act
            var result = await controller.GetGameFileByKey(key);

            // Assert
            mockGenerateGameFile.Verify(s => s.GenerateFileDto(expectedGameDto), Times.Once);
            var fileResult = Assert.IsType<FileContentResult>(result);

            Assert.Equal(fileDto.Content, fileResult.FileContents);
            Assert.Equal(fileDto.FileName, fileResult.FileDownloadName);
            Assert.Equal("text/plain", fileResult.ContentType);
        }

        [Theory]
        [InlineData("game-key-123")]
        public async Task GetGenreByKey_ReturnOk(string key)
        {
            var games = expectedGenreDtos;

            mockGenreService
                .Setup(s => s.GetGenresByGameKeyAsync(key))
                .ReturnsAsync(expectedGenreDtos);

            var controller = CreateController();

            // Act
            var result = await controller.GetGenreByGameKey(key);

            // Assert
            mockGenreService.Verify(s => s.GetGenresByGameKeyAsync(key), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedGenres = Assert.IsType<List<GenreDto>>(okResult.Value);

            Assert.Equal(expectedGenreDtos.Count, returnedGenres.Count);
            for (int i = 0; i < returnedGenres.Count; i++)
            {
                Assert.Equal(expectedGenreDtos[i].Id, returnedGenres[i].Id);
                Assert.Equal(expectedGenreDtos[i].Name, returnedGenres[i].Name);
            }
        }

        [Theory]
        [InlineData("game-key-123")]
        public async Task GetPlatoformByKey_ReturnOk(string key)
        {
            var games = expectedPlatformDtos;

            mockPlatformService
                .Setup(s => s.GetPlatformsByGameKeyAsync(key))
                .ReturnsAsync(expectedPlatformDtos);

            var controller = CreateController();

            // Act
            var result = await controller.GetPlatformsByGameKey(key);

            // Assert
            mockPlatformService.Verify(s => s.GetPlatformsByGameKeyAsync(key), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFullGenres = Assert.IsType<List<PlatformFullDto>>(okResult.Value);

            Assert.Equal(expectedPlatformDtos.Count, returnedFullGenres.Count);
            for (int i = 0; i < returnedFullGenres.Count; i++)
            {
                Assert.Equal(expectedPlatformDtos[i].Id, returnedFullGenres[i].Id);
                Assert.Equal(expectedPlatformDtos[i].Type, returnedFullGenres[i].Type);
            }
        }
    }
}