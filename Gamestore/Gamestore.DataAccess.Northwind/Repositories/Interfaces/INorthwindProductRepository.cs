using Gamestore.DataAccess.Northwind.Entities;
using Gamestore.Domain.Models.DTO.Game;

namespace Gamestore.DataAccess.Northwind.Repositories.Interfaces;

public interface INorthwindProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync(GetGamesRequest request);

    Task<bool> DeleteByKeyAsync(string key);

    Task SetUnitsInStockAsync(string key, int quantityToDeduct);

    Task<IEnumerable<Product>> GetBySupplierNameAsync(string companyName);

    Task<Product> GetAsync(string gameKey);

    Task<Product> GetAsync(int id);

    Task<IEnumerable<Product>> GetByCategoryAsync(int id);

    Task<IEnumerable<Product>> GetBySupplierAsync(int id);

    Task<bool> GameKeyExistAsync(string key);
}
