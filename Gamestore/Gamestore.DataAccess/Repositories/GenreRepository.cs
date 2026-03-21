using Gamestore.DataAccess.Context;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DataAccess.Repositories;

public class GenreRepository(GamestoreDbContext context) : IGenreRepository
{
    private readonly GamestoreDbContext _context = context;

    public async Task CreateGenreAsync(Genre entity)
    {
        await _context.Genres.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> GenreExistsAsync(Guid id)
    {
        return await _context.Genres.AnyAsync(g => g.Id == id);
    }

    public async Task<IEnumerable<Genre>> GetAllGenresAsync()
    {
        return await _context.Genres.ToListAsync();
    }

    public async Task<IEnumerable<Genre?>> GetGenresByGameKeyAsync(string key)
    {
        return await _context.Genres
        .Where(p => p.GameGenres.Any(gp => gp.Game.Key == key))
        .ToListAsync();
    }

    public async Task<Guid?> GetIdByLegacyIdAsync(int id)
    {
        return await _context.Genres.
            AsNoTracking()
            .Where(p => p.LegacyId == id)
            .Select(p => p.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<Genre> GetGenreByIdAsync(Guid id)
    {
        return await _context.Genres.FindAsync(id);
    }

    public async Task<IEnumerable<Genre>> GetGenresByParentIdAsync(Guid id)
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

    public async Task UpdateGenreAsync(Genre entity)
    {
        _context.Genres.Update(entity);
        await _context.SaveChangesAsync();
    }
}
