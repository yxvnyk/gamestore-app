using Gamestore.DataAccess.Entities;

namespace Gamestore.DataAccess.Repositories.Interfaces;

public interface IGameRepository : ICrud
{
    Task CreateGameAsync(GameEntity entity);

    Task<GameEntity?> GetGameByKeyAsync(string key);
}
