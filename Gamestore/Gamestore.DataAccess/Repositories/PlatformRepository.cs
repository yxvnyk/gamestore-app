using Gamestore.DataAccess.Context;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DataAccess.Repositories;

public class PlatformRepository(GamestoreDbContext context) : IPlatformRepository
{
    private readonly GamestoreDbContext _context = context;

    public async Task CreatePlatformAsync(PlatformEntity entity)
    {
        await _context.Platforms.AddAsync(entity);
        _context.SaveChanges();
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<PlatformEntity>> GetAllPlatformsAsync()
    {
        return await _context.Platforms.ToListAsync();
    }

    public async Task<PlatformEntity> GetPlatformByIdAsync(Guid id)
    {
        return await _context.Platforms.FindAsync(id);
    }

    public async Task<IEnumerable<PlatformEntity>> GetPlatformsByGameKeyAsync(string key)
    {
        return await _context.Platforms
        .Where(p => p.GamePlatforms.Any(gp => gp.Game.Key == key))
        .ToListAsync();
    }

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
