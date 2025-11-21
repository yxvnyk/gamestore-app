using System.Text.Json;
using Gamestore.WebApi.Models.Models.DTO;
using Gamestore.WebApi.Services.Interfaces;

namespace Gamestore.WebApi.Services;

public class GenerateGameFile : IGenerateGameFile
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
    };

    public FileDto GenerateFileDto(GameDto game)
    {
        var fileName = $"_{game.Key}.txt";

        var json = JsonSerializer.Serialize(
            game,
            _jsonOptions);

        return new FileDto()
        {
            Content = System.Text.Encoding.UTF8.GetBytes(json),
            FileName = fileName,
        };
    }
}
