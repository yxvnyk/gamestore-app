using Gamestore.DataAccess.Northwind.Entities;
using MongoDB.Bson;

namespace Gamestore.DataAccess.Northwind.Repositories.Interfaces;

public interface INorthwindProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();

    Task<bool> DeleteByKeyAsync(string key);

    Task UpdateAsync(BsonDocument productDoc, string gameKey);
}
