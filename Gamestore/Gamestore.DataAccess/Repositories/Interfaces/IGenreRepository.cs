using Gamestore.DataAccess.Entities;

namespace Gamestore.DataAccess.Repositories.Interfaces;

public interface IGenreRepository : ICrud
{
    Task<bool> GenreExistsAsync(Guid id);

    Task CreateGenreAsync(GenreEntity entity);
}
