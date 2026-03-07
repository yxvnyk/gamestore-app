using Gamestore.DataAccess.Northwind.Context;
using Gamestore.DataAccess.Northwind.Entities;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Gamestore.DataAccess.Northwind.Repositories;

public class NorthwindOrderDetailsRepository(NorthwindDbContext context) : INorthwindOrderDetailsRepository
{
    private readonly NorthwindDbContext _context = context;

    public async Task<IEnumerable<OrderDetails>> GetByOrderIdAsync(int id)
    {
        var query = _context.OrderDetails.AsQueryable()
            .Where(od => od.OrderId == id);

        return await query.ToListAsync();
    }
}