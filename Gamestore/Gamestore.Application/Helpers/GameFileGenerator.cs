using System.Text.Json;
using GameStore.Application.Helpers.Interfaces;
using Gamestore.Domain.Models.DTO;
using Gamestore.Domain.Models.DTO.Game;

namespace GameStore.Application.Helpers;

public class GameFileGenerator : IGenerateGameFile
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
