using Gamestore.Domain.Models.DTO;

namespace Gamestore.Application.Services.Interfaces;

public interface IPlatformService
{
    Task CreatePlatformAsync(PlatformDto model);

    Task<PlatformFullDto?> GetPlatformByIdAsync(Guid id);

    Task<IEnumerable<PlatformFullDto>> GetAllPlatformsAsync();

    Task<IEnumerable<PlatformFullDto>> GetPlatformsByGameKeyAsync(string key);

    Task UpdatePlatformAsync(PlatformUpdateDto model);

    Task<bool> DeleteByIdAsync(Guid id);
}
