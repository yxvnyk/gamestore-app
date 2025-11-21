using Gamestore.DataAccess.Entities;

namespace Gamestore.DataAccess.Repositories.Interfaces;

public interface IGenreRepository : ICrud
{
    Task<bool> GenreExistsAsync(Guid id);

    Task CreateGenreAsync(GenreEntity entity);

    Task<GenreEntity> GetGenreByIdAsync(Guid id);

    Task<IEnumerable<GenreEntity>> GetAllGenresAsync();

    Task<IEnumerable<GenreEntity?>> GetGenreByGameKeyAsync(string key);

    Task<IEnumerable<GenreEntity>> GetGenresByParentIdAsync(Guid id);

    Task SaveChangesAsync();

    Task<bool> DeleteByIdAsync(Guid id);
}
