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

    public async Task<GenreEntity?> GetGenreByGameKeyAsync(string key)
    {
        return await _context.Genres
        .Where(g => g.GameGenres.Any(gg => gg.Game.Key == key))
        .FirstOrDefaultAsync();
    }

    public async Task<GenreEntity> GetGenreByIdAsync(Guid id)
    {
        return await _context.Genres.FindAsync(id);
    }

    public async Task<IEnumerable<GenreEntity>> GetGenresByParentIdAsync(Guid id)
    {
        return await _context.Genres.Where(g => g.ParentGenreId == id).ToListAsync();
    }
}
