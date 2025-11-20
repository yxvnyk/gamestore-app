using Gamestore.WebApi.Models.Models.DTO;

namespace Gamestore.WebApi.Services.Interfaces;

public interface IGameDatabaseService : ICrud
{
    Task CreateGameAsync(GameCreateExtendedDto game);

    Task<GameDto> GetGameAsync(string key);

    Task<GameDto> GetGameAsync(Guid id);

    Task<ICollection<GameDto>> GetGamesByGenreAsync(Guid id);

    Task<ICollection<GameDto>> GetGamesByPlatformAsync(Guid id);

    Task<ICollection<GameDto>> GetAllGamesAsync();

    Task<bool> UpdateGameAsync(GameUpdateExtendedDto model);

    Task<bool> DeleteByKeyAsync(string key);
}
