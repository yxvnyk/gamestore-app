using Gamestore.Domain.Models.DTO.Game;

namespace Gamestore.Application.Services.Interfaces;

public interface IGameService
{
    Task CreateGameAsync(CreateGameRequest createRequest);

    Task<GameDto> GetGameAsync(string key);

    Task<GameDto> GetGameAsync(Guid id);

    Task<ICollection<GameDto>> GetGamesByGenreAsync(Guid id);

    Task<ICollection<GameDto>> GetGamesByPlatformAsync(Guid id);

    Task<ICollection<GameDto>> GetGamesByCompanyNameAsync(string companyName);

    Task<GetGamesResponse> GetAllGamesAsync(GetGamesRequest request);

    Task UpdateGameAsync(UpdateGameRequest updateRequest);

    Task<bool> DeleteByKeyAsync(string key);

    Task<int> GetTotalGamesCountAsync();
}
