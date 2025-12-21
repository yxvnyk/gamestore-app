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
using System.Text;
using System.Text.Json;

namespace Gamestore.Application.UnitTests.Helpers
{
    public class GenerateGameFileTest
    {
        private readonly Mock<IGameRepository> _mockGameRepo = new();

        private static GameDto CreateGameDto(
            string name = "Game",
            string key = "key",
            string description = "Desc")
            => new ()
            {
                Name = name,
                Description = description,
                Key = key
            };
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true,
        };

        [Fact]
        public void GenerateFileDto()
        {
            var gameDto = CreateGameDto();

            var expectedKey = "_key.txt";
            var expectedJson = JsonSerializer.Serialize(gameDto, _jsonOptions);
            var generator = new GenerateGameFile();

            var fileDto = generator.GenerateFileDto(gameDto);


            Assert.NotNull(fileDto.Content);

            var actualString = Encoding.UTF8.GetString(fileDto.Content);
            Assert.Equal(expectedJson, actualString);
            Assert.Contains("\"Name\": \"Game\"", actualString);
            Assert.Contains("\"Key\": \"key\"", actualString);
            Assert.Contains("\"Description\": \"Desc\"", actualString);
        }
    }
}
