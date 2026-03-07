using Gamestore.Application.Models;
using Gamestore.Domain.Models.DTO.Game;

namespace Gamestore.Application.Services.Interfaces;

public interface IGameService
{
    Task CreateGameAsync(CreateGameRequest createRequest);

    Task<GameDto> GetAsync(string key);

    Task<GameDto> GetByIdAsync(Identity id);

    Task<ICollection<GameDto>> GetByGenreAsync(Identity id);

    Task<ICollection<GameDto>> GetByPlatformAsync(Guid id);

    Task<ICollection<GameDto>> GeByCompanyNameAsync(string companyName);

    Task<GetGamesResponse> GetAllGamesAsync(GetGamesRequest request);

    Task UpdateGameAsync(UpdateGameRequest updateRequest);

    Task<bool> DeleteByKeyAsync(string key);

    Task<int> GetTotalGamesCountAsync();
}
