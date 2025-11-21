using Gamestore.DataAccess.Context;
using Gamestore.DataAccess.Repositories.Interfaces;

namespace Gamestore.DataAccess.Repositories;

public class PlatformRepository(GamestoreDbContext context) : IPlatformRepository
{
    private readonly GamestoreDbContext _context = context;

    public async Task<bool> PlatformExistsAsync(Guid id)
    {
        var exist = await _context.Platforms.FindAsync(id);
        return exist != null;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
