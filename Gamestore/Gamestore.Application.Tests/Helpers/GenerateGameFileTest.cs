using System.Text;
using System.Text.Json;
using GameStore.Application.Helpers;
using Gamestore.Domain.Models.DTO;

namespace Gamestore.Application.UnitTests.Helpers;

public class GenerateGameFileTest
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
    };

    [Fact]
    public void GenerateFileDto()
    {
        var gameDto = CreateGameDto();
        var expectedJson = JsonSerializer.Serialize(gameDto, JsonOptions);
        var generator = new GenerateGameFile();

        var fileDto = generator.GenerateFileDto(gameDto);

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
