using Gamestore.DataAccess.Northwind.Context;
using Gamestore.DataAccess.Northwind.Entities;
using Gamestore.DataAccess.Northwind.Extension;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using Gamestore.Domain.Interfaces;
using Gamestore.Domain.Models.DTO.Game;
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

    public async Task<IEnumerable<Product>> GetByCategoryAsync(int id)
    {
        var categoryExist = await _context.Categories.AsQueryable()
        .Where(s => s.CategoryId == id)
        .AnyAsync();

        if (!categoryExist)
        {
            return [];
        }

        var query = _context.Products.AsQueryable()
            .Where(p => p.CategoryId == id);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetBySupplierAsync(int id)
    {
        var supplierExist = await _context.Suppliers.AsQueryable()
        .Where(s => s.SupplierId == id)
        .AnyAsync();

        if (!supplierExist)
        {
            return [];
        }

        var query = _context.Products.AsQueryable()
            .Where(p => p.SupplierId == id);

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

    public async Task SetUnitsInStockAsync(string key, int quantityToDeduct)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.GameKey, key);

        var update = Builders<Product>.Update.Set(p => p.UnitsInStock, -quantityToDeduct);

        await _context.Products.UpdateOneAsync(filter, update);
    }
}
