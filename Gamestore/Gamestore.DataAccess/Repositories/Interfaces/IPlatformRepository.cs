using Gamestore.DataAccess.Entities;

namespace Gamestore.DataAccess.Repositories.Interfaces;

public interface IPlatformRepository : ICrud
{
    Task<bool> PlatformExistsAsync(Guid id);

    Task SaveChangesAsync();

    Task CreatePlatformAsync(PlatformEntity entity);

    Task<PlatformEntity> GetPlatformByIdAsync(Guid id);

    Task<IEnumerable<PlatformEntity>> GetAllPlatformsAsync();

    Task<IEnumerable<PlatformEntity>> GetPlatformsByGameKeyAsync(string key);

    Task<bool> DeleteByIdAsync(Guid id);
}
