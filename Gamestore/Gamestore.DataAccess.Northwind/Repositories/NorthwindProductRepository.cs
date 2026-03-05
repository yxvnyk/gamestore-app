using Gamestore.DataAccess.Northwind.Context;
using Gamestore.DataAccess.Northwind.Entities;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using Gamestore.Domain.Interfaces;
using MongoDB.Bson;
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

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.Find(_ => true).ToListAsync();
    }

    public async Task<bool> DeleteByKeyAsync(string key)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.GameKey, key);
        var result = await _context.Products.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }

    public async Task UpdateAsync(BsonDocument productDoc, string gameKey)
    {
        if (productDoc.ElementCount != 0)
        {
            return;
        }

        var filter = Builders<Product>.Filter.Eq("GameKey", gameKey);
        await _context.Products.UpdateOneAsync(filter, productDoc);
    }
}
