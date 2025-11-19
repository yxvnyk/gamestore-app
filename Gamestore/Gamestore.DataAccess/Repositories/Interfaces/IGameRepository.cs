using Gamestore.DataAccess.Entities;

namespace Gamestore.DataAccess.Repositories.Interfaces;

public interface IGameRepository : ICrud
{
    Task CreateGameAsync(GameEntity entity);

    Task<GameEntity?> GetGameByKeyAsync(string key);

    Task<GameEntity?> GetGameByIdAsync(Guid id);

    Task<ICollection<GameEntity>> GetGamesByGenreAsync(Guid id);

    Task<ICollection<GameEntity>> GetGamesByPlatformAsync(Guid id);

    Task<bool> GameKeyExistAsync(string key);
}
