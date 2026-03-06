using Gamestore.DataAccess.Northwind.Context;
using Gamestore.DataAccess.Northwind.Entities;
using Gamestore.DataAccess.Northwind.Extension;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using Gamestore.Domain.Interfaces;
using Gamestore.Domain.Models.DTO.Game;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Gamestore.DataAccess.Northwind.Repositories;

public class NorthwindProductRepository(NorthwindDbContext context) : INorthwindProductRepository, IUniqueKeyRepository
{
    private readonly NorthwindDbContext _context = context;

    public async Task<bool> GameKeyExistAsync(string key)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.GameKey, key);
        return await _context.Products.Find(filter).AnyAsync();
    }

    public async Task<IEnumerable<Product>> GetAllAsync(GetGamesRequest request)
    {
        var query = _context.Products.AsQueryable();
        query = query
            .FilterByCategories(request.Genres)
            .FilterBySuppliers(request.Publishers)
            .FilterByProductName(request.Name)
            .FilterByPrice(request.MinPrice, request.MaxPrice);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetBySupplierNameAsync(string companyName)
    {
        var supplierId = await _context.Suppliers.AsQueryable()
        .Where(s => s.CompanyName == companyName)
        .Select(s => s.SupplierId)
        .FirstOrDefaultAsync();

        if (supplierId == 0)
        {
            return [];
        }

        var query = _context.Products.AsQueryable()
            .Where(p => p.SupplierId == supplierId);

        return await query.ToListAsync();
    }

    public async Task<Product> GetAsync(string gameKey)
    {
        return await _context.Products.AsQueryable()
            .FirstOrDefaultAsync(p => p.GameKey == gameKey);
    }

    public async Task<Product> GetAsync(int id)
    {
        return await _context.Products.AsQueryable()
            .FirstOrDefaultAsync(p => p.ProductId == id);
    }

    public async Task<bool> DeleteByKeyAsync(string key)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.GameKey, key);
        var result = await _context.Products.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }

    public async Task UpdateAsync(BsonDocument productDoc, string gameKey)
    {
        if (productDoc.ElementCount == 0)
        {
            return;
        }

        var filter = Builders<Product>.Filter.Eq("GameKey", gameKey);
        await _context.Products.UpdateOneAsync(filter, productDoc);
    }
}
