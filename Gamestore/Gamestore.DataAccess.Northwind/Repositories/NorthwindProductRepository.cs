using Gamestore.DataAccess.Northwind.Context;
using Gamestore.DataAccess.Northwind.Entities;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using Gamestore.Domain.Interfaces;
using MongoDB.Driver;

namespace Gamestore.DataAccess.Northwind.Repositories;

public class NorthwindProductRepository(NorthwindDbContext context) : INorthwindProductRepository, IUniqueKeyRepository
{
    private readonly NorthwindDbContext _context = context;

    public async Task<bool> GameKeyExistAsync(string key)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.GameKey, key);
        return await _context.Products.Find(filter).AnyAsync();
    }
}
