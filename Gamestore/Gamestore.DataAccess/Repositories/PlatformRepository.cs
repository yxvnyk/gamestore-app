using Gamestore.DataAccess.Context;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DataAccess.Repositories;

public class PlatformRepository(GamestoreDbContext context) : IPlatformRepository
{
    private readonly GamestoreDbContext _context = context;

    public async Task CreatePlatformAsync(Platform entity)
    {
        _context.Platforms.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var exist = await _context.Platforms.FindAsync(id);
        if (exist != null)
        {
            _ = _context.Platforms.Remove(exist);
            _ = await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<IEnumerable<Platform>> GetAllPlatformsAsync()
    {
        return await _context.Platforms.ToListAsync();
    }

    public async Task<Platform> GetPlatformByIdAsync(Guid id)
    {
        return await _context.Platforms.FindAsync(id);
    }

    public async Task<IEnumerable<Platform>> GetPlatformsByGameKeyAsync(string key)
    {
        return await _context.Platforms
        .Where(p => p.GamePlatforms.Any(gp => gp.Game.Key == key))
        .ToListAsync();
    }

    public async Task<bool> PlatformExistsAsync(Guid id)
    {
        return await _context.Platforms.AnyAsync(p => p.Id == id);
    }

    public async Task UpdatePlatformAsync(Platform entity)
    {
        _ = _context.Platforms.Update(entity);
        await _context.SaveChangesAsync();
    }
}
