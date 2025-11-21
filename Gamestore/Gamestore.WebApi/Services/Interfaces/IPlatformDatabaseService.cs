using Gamestore.WebApi.Models.Models.DTO;

namespace Gamestore.WebApi.Services.Interfaces;

public interface IPlatformDatabaseService
{
    Task CreatePlatformAsync(PlatformDto model);

    Task<PlatformFullDto?> GetPlatformByIdAsync(Guid id);

    Task<IEnumerable<PlatformFullDto>> GetAllPlatformsAsync();

    Task<IEnumerable<PlatformFullDto>> GetPlatformsByGameKeyAsync(string key);

    Task UpdatePlatformAsync(PlatformUpdateDto model);

    Task<bool> DeleteByIdAsync(Guid id);
}
