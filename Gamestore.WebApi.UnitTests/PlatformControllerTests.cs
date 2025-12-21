using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Models.DTO;
using Gamestore.WebApi.Controllers;
using GameStore.Application.Helpers.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Gamestore.WebApi.UnitTests
{
    public class PlatformControllerTests
    {
        private readonly Mock<IGameService> mockGameService = new();
        private readonly Mock<IPlatformService> mockPlatformService = new();

        private PlatformsController CreateController()
        {
            return new PlatformsController(
                mockPlatformService.Object,
                mockGameService.Object);
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
        private readonly List<PlatformFullDto> expectedPlatformDtos = new()
        {
            new PlatformFullDto
            {
                Id = Guid.NewGuid(),
                Type = "IOS"
            },
            new PlatformFullDto
            {
                Id = Guid.NewGuid(),
                Type = "Android"
            },
            new PlatformFullDto
            {
                Id = Guid.NewGuid(),
                Type = "Unix"
            }
        };

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true,
        };

        [Fact]
        public async Task CreateGenre_ReturnOk()
        {
            var dto = new PlatformDto();

            var controller = CreateController();

            // Act
            var result = await controller.CreatePlatform(dto);

            // Assert
            mockPlatformService.Verify(s => s.CreatePlatformAsync(dto), Times.Once);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task GetPlatformById_ReturnOk()
        {
            var id = Guid.NewGuid();
            var expectedPlatformDto = new PlatformFullDto
            { 
                Type = "Sample Platform Name",
                Id = id
            };

            mockPlatformService
                .Setup(s => s.GetPlatformByIdAsync(id))
                .ReturnsAsync(expectedPlatformDto);

            var controller = CreateController();

            // Act
            var result = await controller.GetPlatformById(id);

            // Assert
            mockPlatformService.Verify(s => s.GetPlatformByIdAsync(id), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedGenre = Assert.IsType<PlatformFullDto>(okResult.Value);

            Assert.Equal(expectedPlatformDto.Id, returnedGenre.Id);
            Assert.Equal(expectedPlatformDto.Type, returnedGenre.Type);
        }

        [Fact]
        public async Task GetAllPlatforms_ReturnOk()
        {
            var dtoList = expectedPlatformDtos;

            mockPlatformService
                .Setup(s => s.GetAllPlatformsAsync())
                .ReturnsAsync(dtoList);

            var controller = CreateController();

            // Act
            var result = await controller.GetAllPlatfomrs();

            // Assert
            mockPlatformService.Verify(s => s.GetAllPlatformsAsync(), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedGenres = Assert.IsType<List<PlatformFullDto>>(okResult.Value);

            Assert.Equal(dtoList.Count, returnedGenres.Count);
            for (int i = 0; i < dtoList.Count; i++)
            {
                Assert.Equal(dtoList[i].Id, returnedGenres[i].Id);
                Assert.Equal(dtoList[i].Type, returnedGenres[i].Type);
            }

        }

        [Fact]
        public async Task UpdatePlatforms_ReturnOk()
        {
            var dto = new PlatformUpdateDto();

            var controller = CreateController();

            // Act
            var result = await controller.UpdatePlatform(dto);

            // Assert
            mockPlatformService.Verify(s => s.UpdatePlatformAsync(dto), Times.Once);
            var resultValue = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Platform successfuly updated", resultValue.Value);
        }

        [Fact]  
        public async Task DeleteGenre_SuccessfullyDelete_ReturnNoContent()
        {
            var id = Guid.NewGuid();
            var controller = CreateController();
            mockPlatformService
                .Setup(s => s.DeleteByIdAsync(id))
                .ReturnsAsync(true);

            // Act
            var result = await controller.DeletePlatform(id);

            // Assert
            mockPlatformService.Verify(s => s.DeleteByIdAsync(id), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]  
        public async Task DeleteGenre_NotFound_ReturnNotFound()
        {
            var id = Guid.NewGuid();
            var controller = CreateController();
            mockPlatformService
                .Setup(s => s.DeleteByIdAsync(id))
                .ReturnsAsync(false);

            // Act
            var result = await controller.DeletePlatform(id);

            // Assert
            mockPlatformService.Verify(s => s.DeleteByIdAsync(id), Times.Once);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetGamesByPlatform_ReturnOk()
        {
            var expectedGames = expectedGameDtos;
            var id = Guid.NewGuid();
            var controller = CreateController();
            mockGameService
                .Setup(s => s.GetGamesByPlatformAsync(id))
                .ReturnsAsync(expectedGames);

            // Act
            var result = await controller.GetGamesByPlatform(id);

            // Assert
            mockGameService.Verify(s => s.GetGamesByPlatformAsync(id), Times.Once);
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

    }
}