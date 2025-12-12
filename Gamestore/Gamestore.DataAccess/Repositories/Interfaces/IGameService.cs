using Gamestore.DataAccess.Entities;

namespace Gamestore.DataAccess.Repositories.Interfaces;

public interface IGameService
{
    Task CreateGameAsync(Game entity);

    Task<Game?> GetGameByKeyAsync(string key);

    Task<Game?> GetGameByIdAsync(Guid id);

    Task<ICollection<Game>> GetGamesByGenreAsync(Guid id);

    Task<ICollection<Game>> GetGamesByPlatformAsync(Guid id);

    Task<ICollection<Game>> GetAllGamesAsync();

    Task<Game?> GetGameWithJoinsAsync(Guid id);

    Task<bool> GameKeyExistAsync(string key);

    Task UpdateGameAsync(Game entity);

    Task<bool> DeleteByKeyAsync(string key);

    Task<int> GetTotalGamesCountAsync();
}
