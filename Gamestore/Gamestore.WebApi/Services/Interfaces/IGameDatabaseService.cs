using Gamestore.WebApi.Models.Models.DTO;

namespace Gamestore.WebApi.Services.Interfaces;

public interface IGameDatabaseService : ICrud
{
    Task CreateGameAsync(GameCreateDto game);

    Task<GameDto> GetGameAsync(string key);

    Task<GameDto> GetGameAsync(Guid id);

    Task<ICollection<GameDto>> GetGamesByGenreAsync(Guid id);

    Task<ICollection<GameDto>> GetGamesByPlatformAsync(Guid id);
}
