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
}
