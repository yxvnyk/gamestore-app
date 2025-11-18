using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.WebApi.Models.Models.DTO;
using Gamestore.WebApi.Services.Interfaces;

namespace Gamestore.WebApi.Services;

public class GameDatabaseService(IGameRepository gameRepository) : IGameDatabaseService
{
    private readonly IGameRepository _gameRepository = gameRepository;

    public async Task CreateGameAsync(GameCreateDto game)
    {
        GameEntity gameEntity = new()
        {
            Name = game.Game.Name,
            Key = game.Game.Key,
            Description = game.Game.Description,
            GameGenres = [.. game.Genres.Select(genreId => new GameGenreEntity { GenreId = genreId })],
            GamePlatforms = [.. game.Platforms.Select(platformId => new GamePlatformEntity { PlatformId = platformId })],
        };
        await _gameRepository.CreateGameAsync(gameEntity);
    }
}
