using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Models;
using Gamestore.Domain.Models.DTO.Game;

namespace Gamestore.DataAccess.Repositories.Interfaces;

public interface IGameRepository
{
    Task CreateGameAsync(Game entity);

    Task<Game?> GetGameByKeyAsync(string key);

    Task<Game?> GetGameByIdAsync(Guid id);

    Task<ICollection<Game>> GetGamesByGenreAsync(Guid id);

    Task<ICollection<Game>> GetGamesByPlatformAsync(Guid id);

    Task<ICollection<Game>> GetGamesByCompanyNameAsync(string companyName);

    Task<PagedList<Game>> GetAllGamesAsync(GetGamesRequest request);

    Task<Guid?> GetGameIdByKeyAsync(string key);

    Task<Game?> GetGameWithJoinsAsync(Guid id);

    Task<bool> GameKeyExistAsync(string key);

    Task<int> GetUnitsInStockAsync(Guid gameId);

    Task UpdateGameAsync(Game entity);

    Task<bool> DeleteByKeyAsync(string key);

    Task<int> GetTotalGamesCountAsync();

    Task<Guid?> GetIdByKeyAsync(string key);
}
