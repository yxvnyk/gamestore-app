using Gamestore.DataAccess.Context;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DataAccess.Repositories;

public class GameRepository(GamestoreDbContext context) : IGameRepository
{
    private readonly GamestoreDbContext _context = context;

    public async Task CreateGameAsync(GameEntity entity)
    {
        await _context.Games.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<GameEntity?> GetGameByKeyAsync(string key)
    {
        var game = await _context.Games.FirstOrDefaultAsync(g => g.Key == key);
        return game;
    }

    public async Task<bool> GameKeyExistAsync(string key)
    {
        var exist = await _context.Games.AnyAsync(g => g.Key == key);
        return exist;
    }

    public async Task<ICollection<GameEntity>> GetGamesByPlatformAsync(Guid id)
    {
        return await _context.Games
            .Include(g => g.GamePlatforms)
            .ThenInclude(gp => gp.Platform)
            .Where(g => g.GamePlatforms.Any(gp => gp.PlatformId == id)).ToListAsync();
    }

    public async Task<ICollection<GameEntity>> GetGamesByGenreAsync(Guid id)
    {
        return await _context.Games
            .Include(g => g.GameGenres)
            .ThenInclude(gp => gp.Genre)
            .Where(g => g.GameGenres.Any(gp => gp.GenreId == id)).ToListAsync();
    }

    public async Task<GameEntity?> GetGameByIdAsync(Guid id)
    {
        var game = await _context.Games.FindAsync(id);
        return game;
    }

    public async Task<GameEntity?> GetGameWithJoinsAsync(Guid id)
    {
        var game = await _context.Games
            .Include(g => g.GameGenres)
            .Include(p => p.GamePlatforms)
            .FirstOrDefaultAsync(g => g.Id == id);
        return game;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteByKeyAsync(string key)
    {
        var exist = await _context.Games.FirstOrDefaultAsync(g => g.Key == key);
        if (exist != null)
        {
            _ = _context.Games.Remove(exist);
            _ = await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}
