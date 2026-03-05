using Gamestore.DataAccess.Northwind.Entities;
using Gamestore.Domain.Models.DTO.Game;
using MongoDB.Bson;

namespace Gamestore.DataAccess.Northwind.Repositories.Interfaces;

public interface INorthwindProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync(GetGamesRequest request);

    Task<bool> DeleteByKeyAsync(string key);

    Task UpdateAsync(BsonDocument productDoc, string gameKey);
}
