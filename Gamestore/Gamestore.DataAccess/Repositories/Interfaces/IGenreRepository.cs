using Gamestore.DataAccess.Entities;

namespace Gamestore.DataAccess.Repositories.Interfaces;

public interface IGenreRepository
{
    Task<bool> GenreExistsAsync(Guid id);

    Task CreateGenreAsync(Genre entity);

    Task<Genre> GetGenreByIdAsync(Guid id);

    Task<IEnumerable<Genre>> GetAllGenresAsync();

    Task<IEnumerable<Genre?>> GetGenresByGameKeyAsync(string key);

    Task<IEnumerable<Genre>> GetGenresByParentIdAsync(Guid id);

    Task<Guid?> GetIdByLegacyIdAsync(int id);

    Task UpdateGenreAsync(Genre entity);

    Task<bool> DeleteByIdAsync(Guid id);
}
