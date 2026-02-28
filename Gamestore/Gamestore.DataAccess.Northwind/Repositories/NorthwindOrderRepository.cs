using Gamestore.DataAccess.Northwind.Context;
using Gamestore.DataAccess.Northwind.Entities;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Gamestore.DataAccess.Northwind.Repositories;

public class NorthwindOrderRepository(NorthwindDbContext context) : INorthwindOrderRepository
{
    private readonly NorthwindDbContext _context = context;

    public async Task<IEnumerable<Order>> GetHistoryAsync()
    {
        var queriableCollection = _context.Orders.AsQueryable();
        var query = from order in queriableCollection
                    select new Order()
                    { CustomerId = order.CustomerId, OrderDate = order.OrderDate, OrderId = order.OrderId };

        return await query.ToListAsync();
    }
}