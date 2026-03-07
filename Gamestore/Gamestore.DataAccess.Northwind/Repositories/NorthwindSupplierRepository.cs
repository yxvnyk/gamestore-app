using Gamestore.DataAccess.Northwind.Context;
using Gamestore.DataAccess.Northwind.Entities;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Gamestore.DataAccess.Northwind.Repositories;

public class NorthwindSupplierRepository(NorthwindDbContext context) : INorthwindSupplierRepository
{
    private readonly NorthwindDbContext _context = context;

    public async Task<bool> GameKeyExistAsync(string key)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.GameKey, key);
        return await _context.Products.Find(filter).AnyAsync();
    }

    public async Task<Supplier?> GetByGameKeyAsync(string key)
    {
        var supplierId = await _context.Products.AsQueryable()
            .Where(p => p.GameKey == key)
            .Select(p => p.SupplierId)
            .FirstOrDefaultAsync();

        if (supplierId == 0)
        {
            return null;
        }

        // retunrn first or default
        return await _context.Suppliers.AsQueryable()
        .Where(c => c.SupplierId == supplierId)
        .FirstOrDefaultAsync();
    }

    public async Task<Supplier?> GetByCompanyNameAsync(string name)
    {
        var supplier = await _context.Suppliers.AsQueryable()
            .Where(p => p.ContactName == name)
            .FirstOrDefaultAsync();

        return supplier;
    }

    public async Task<IEnumerable<Supplier>> GetAllAsync()
    {
        return await _context.Suppliers.AsQueryable().ToListAsync();
    }

    public async Task<Supplier>? GetAsync(int id)
    {
        return await _context.Suppliers.AsQueryable()
            .FirstOrDefaultAsync(c => c.SupplierId == id);
    }
}