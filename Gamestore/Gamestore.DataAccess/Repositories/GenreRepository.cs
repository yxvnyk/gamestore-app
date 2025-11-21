using Gamestore.DataAccess.Context;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DataAccess.Repositories;

public class GenreRepository(GamestoreDbContext context) : IGenreRepository
{
    private readonly GamestoreDbContext _context = context;

    public async Task CreateGenreAsync(GenreEntity entity)
    {
        await _context.Genres.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> GenreExistsAsync(Guid id)
    {
        var exist = await _context.Genres.FindAsync(id);
        return exist != null;
    }

    public async Task<IEnumerable<GenreEntity>> GetAllGenresAsync()
    {
        return await _context.Genres.ToListAsync();
    }

    public async Task<IEnumerable<GenreEntity?>> GetGenreByGameKeyAsync(string key)
    {
        return await _context.Genres
        .Where(p => p.GameGenres.Any(gp => gp.Game.Key == key))
        .ToListAsync();
    }

    public async Task<GenreEntity> GetGenreByIdAsync(Guid id)
    {
        return await _context.Genres.FindAsync(id);
    }

    public async Task<IEnumerable<GenreEntity>> GetGenresByParentIdAsync(Guid id)
    {
        return await _context.Genres.Where(g => g.ParentGenreId == id).ToListAsync();
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var exist = await _context.Genres.FindAsync(id);
        if (exist != null)
        {
            _ = _context.Genres.Remove(exist);
            _ = await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
