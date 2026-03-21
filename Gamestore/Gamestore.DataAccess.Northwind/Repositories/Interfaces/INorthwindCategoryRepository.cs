using Gamestore.DataAccess.Northwind.Entities;

namespace Gamestore.DataAccess.Northwind.Repositories.Interfaces;

public interface INorthwindCategoryRepository
{
    Task<bool> GameKeyExistAsync(string key);

    Task<Category?> GetByGameKeyAsync(string key);

    Task<IEnumerable<Category>> GetAllAsync();

    Task<Category>? GetAsync(int id);

    Task<bool> CategoryExistsAsync(int id);
}
