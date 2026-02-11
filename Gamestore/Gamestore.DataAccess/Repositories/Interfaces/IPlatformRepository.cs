using Gamestore.DataAccess.Entities;

namespace Gamestore.DataAccess.Repositories.Interfaces;

public interface IPlatformRepository
{
    Task<bool> PlatformExistsAsync(Guid id);

    Task UpdatePlatformAsync(Platform entity);

    Task CreatePlatformAsync(Platform entity);

    Task<Platform> GetPlatformByIdAsync(Guid id);

    Task<IEnumerable<Platform>> GetAllPlatformsAsync();

    Task<IEnumerable<Platform>> GetPlatformsByGameKeyAsync(string key);

    Task<bool> DeleteByIdAsync(Guid id);
}
