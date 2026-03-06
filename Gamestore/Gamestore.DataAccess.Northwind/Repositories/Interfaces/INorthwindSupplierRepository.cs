using Gamestore.DataAccess.Northwind.Entities;

namespace Gamestore.DataAccess.Northwind.Repositories.Interfaces;

public interface INorthwindSupplierRepository
{
    Task<bool> GameKeyExistAsync(string key);

    Task<Supplier?> GetByGameKeyAsync(string key);
}
