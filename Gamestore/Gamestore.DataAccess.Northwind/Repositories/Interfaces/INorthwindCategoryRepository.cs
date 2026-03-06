using Gamestore.DataAccess.Northwind.Entities;

namespace Gamestore.DataAccess.Northwind.Repositories.Interfaces;

public interface INorthwindCategoryRepository
{
    Task<bool> GameKeyExistAsync(string key);

    Task<Category?> GetByGameKeyAsync(string key);
}
