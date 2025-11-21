using Gamestore.WebApi.Models.Models.DTO;

namespace Gamestore.WebApi.Services.Interfaces;

public interface IGameDatabaseService
{
    Task CreateGameAsync(GameCreateExtendedDto game);

    Task<GenreDto> GetGameAsync(string key);

    Task<GenreDto> GetGameAsync(Guid id);

    Task<ICollection<GenreDto>> GetGamesByGenreAsync(Guid id);

    Task<ICollection<GenreDto>> GetGamesByPlatformAsync(Guid id);

    Task<ICollection<GenreDto>> GetAllGamesAsync();

    Task UpdateGameAsync(GameUpdateExtendedDto model);

    Task<bool> DeleteByKeyAsync(string key);
}
