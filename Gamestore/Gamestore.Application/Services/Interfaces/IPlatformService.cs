using Gamestore.Domain.Models.DTO.Platform;

namespace Gamestore.Application.Services.Interfaces;

public interface IPlatformService
{
    Task CreatePlatformAsync(PlatformCreateDto model);

    Task<PlatformDto?> GetPlatformByIdAsync(Guid id);

    Task<IEnumerable<PlatformDto>> GetAllPlatformsAsync();

    Task<IEnumerable<PlatformDto>?> GetPlatformsByGameKeyAsync(string key);

    Task UpdatePlatformAsync(PlatformUpdateDto model);

    Task<bool> DeleteByIdAsync(Guid id);
}
