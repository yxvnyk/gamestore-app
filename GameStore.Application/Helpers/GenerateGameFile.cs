using System.Text.Json;
using Gamestore.Domain.Models.DTO;
using GameStore.Application.Helpers.Interfaces;

namespace GameStore.Application.Helpers;

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
