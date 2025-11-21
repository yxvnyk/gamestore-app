namespace Gamestore.DataAccess.Repositories.Interfaces;

public interface IPlatformRepository : ICrud
{
    Task<bool> PlatformExistsAsync(Guid id);

    Task SaveChangesAsync();
}
