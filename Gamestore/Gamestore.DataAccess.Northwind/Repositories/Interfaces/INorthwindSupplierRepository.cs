using Gamestore.DataAccess.Northwind.Entities;

namespace Gamestore.DataAccess.Northwind.Repositories.Interfaces;

public interface INorthwindSupplierRepository
{
    Task<bool> GameKeyExistAsync(string key);

    Task<Supplier?> GetByGameKeyAsync(string key);

    Task<IEnumerable<Supplier>> GetAllAsync();

    Task<Supplier>? GetAsync(int id);

    Task<Supplier?> GetByCompanyNameAsync(string name);

    Task<bool> SupplierExistsAsync(int id);
}
