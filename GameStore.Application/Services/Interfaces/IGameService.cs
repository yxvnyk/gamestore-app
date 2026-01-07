using Gamestore.Domain.Models.DTO;

namespace Gamestore.Application.Services.Interfaces;

public interface IGameService
{
    Task CreateGameAsync(GameCreateExtendedDto game);

    Task<GameDto> GetGameAsync(string key);

    Task<GameDto> GetGameAsync(Guid id);

    Task<ICollection<GameDto>> GetGamesByGenreAsync(Guid id);

    Task<ICollection<GameDto>> GetGamesByPlatformAsync(Guid id);

    Task<ICollection<GameDto>> GetAllGamesAsync();

    Task UpdateGameAsync(GameUpdateExtendedDto model);

    Task<bool> DeleteByKeyAsync(string key);
}
