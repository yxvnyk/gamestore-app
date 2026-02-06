using System.Text;
using System.Text.Json;
using Gamestore.Application.Helpers.Generators;
using Gamestore.Domain.Models.DTO.Game;

namespace Gamestore.Application.Tests.Helpers;

public class GenerateGameFileTest
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
    };

    [Fact]
    public void GenerateFileDto()
    {
        // Arrange
        var gameDto = CreateGameDto();
        var expectedJson = JsonSerializer.Serialize(gameDto, JsonOptions);
        var generator = new GameFileGenerator();

        // Act
        var fileDto = generator.GenerateFileDto(gameDto);

        // Assert
        Assert.NotNull(fileDto.Content);

        var actualString = Encoding.UTF8.GetString(fileDto.Content);
        Assert.Equal(expectedJson, actualString);
        Assert.Contains("\"Name\": \"Game\"", actualString);
        Assert.Contains("\"Key\": \"key\"", actualString);
        Assert.Contains("\"Description\": \"Desc\"", actualString);
    }

    private static GameDto CreateGameDto(
        string name = "Game",
        string key = "key",
        string description = "Desc")
        => new()
        {
            Name = name,
            Description = description,
            Key = key,
        };
}