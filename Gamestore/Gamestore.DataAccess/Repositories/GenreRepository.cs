using Gamestore.DataAccess.Context;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;

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
}
