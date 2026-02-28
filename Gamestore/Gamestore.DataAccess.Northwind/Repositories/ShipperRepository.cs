using Gamestore.DataAccess.Northwind.Context;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Gamestore.DataAccess.Northwind.Repositories;

public class ShipperRepository(NorthwindDbContext context) : IShipperRepository
{
    private readonly NorthwindDbContext _context = context;

    public async Task<IEnumerable<dynamic>> GetAllAsync()
    {
        var queriableCollection = _context.Shippers.AsQueryable();
        var bsonDocuments = await queriableCollection.ToListAsync();

        var result = bsonDocuments.Select(d => d.ToDictionary(
                el => el.Name,
                el => BsonTypeMapper.MapToDotNetValue(el.Value)));

        return result;
    }
}