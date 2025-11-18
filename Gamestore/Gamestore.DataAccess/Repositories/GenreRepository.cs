using Gamestore.DataAccess.Context;
using Gamestore.DataAccess.Repositories.Interfaces;

namespace Gamestore.DataAccess.Repositories;

public class GenreRepository(GamestoreDbContext context) : IGenreRepository
{
    private readonly GamestoreDbContext _context = context;

    public async Task<bool> GenreExistsAsync(Guid id)
    {
        var exist = await _context.Genres.FindAsync(id);
        return exist != null;
    }
}
