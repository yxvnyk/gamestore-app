using Gamestore.DataAccess.Northwind.Context;
using Gamestore.DataAccess.Northwind.Entities;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Gamestore.DataAccess.Northwind.Repositories;

public class NorthwindCategoryRepository(NorthwindDbContext context) : INorthwindCategoryRepository
{
    private readonly NorthwindDbContext _context = context;

    public async Task<bool> GameKeyExistAsync(string key)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.GameKey, key);
        return await _context.Products.Find(filter).AnyAsync();
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories.AsQueryable().ToListAsync();
    }

    public async Task<Category>? GetAsync(int id)
    {
        return await _context.Categories.AsQueryable()
            .FirstOrDefaultAsync(c => c.CategoryId == id);
    }

    public async Task<Category?> GetByGameKeyAsync(string key)
    {
        var categoryId = await _context.Products.AsQueryable()
            .Where(p => p.GameKey == key)
            .Select(p => p.CategoryId)
            .FirstOrDefaultAsync();

        if (categoryId == 0)
        {
            return null;
        }

        // retunrn first or default
        return await _context.Categories.AsQueryable()
        .Where(c => c.CategoryId == categoryId)
        .FirstOrDefaultAsync();
    }
}
