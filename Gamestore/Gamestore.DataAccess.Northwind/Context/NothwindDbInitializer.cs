using Gamestore.DataAccess.Northwind.Entities;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using Gamestore.Domain.Generators;
using Gamestore.Domain.Interfaces;
using MongoDB.Driver;

namespace Gamestore.DataAccess.Northwind.Context;

public class NothwindDbInitializer(NorthwindDbContext northwindDbContext, INorthwindProductRepository productRepository, IKeyGenerator keyGenerator)
{
    private readonly NorthwindDbContext _context = northwindDbContext;
    private readonly IKeyGenerator _keyGenerator = keyGenerator;
    private readonly INorthwindProductRepository _productRepository = productRepository;

    public async Task InitializeGameKeyAync()
    {
        var filter = Builders<Product>.Filter.Exists(p => p.GameKey, false);
        var needMigration = await _context.Products.Find(filter).AnyAsync();

        if (!needMigration)
        {
            return;
        }

        var products = await _context.Products.Find(filter).ToListAsync();
        foreach (var product in products)
        {
            var gameKey = await _keyGenerator.GenerateUniqueKeyAsync((IUniqueKeyRepository)_productRepository, product.ProductName);
            var update = Builders<Product>.Update.Set(p => p.GameKey, gameKey);
            await _context.Products.UpdateOneAsync(p => p.ProductId == product.ProductId, update);
        }
    }
}
