using Gamestore.DataAccess.Context;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DataAccess.Repositories;

public class OrderRepository(GamestoreDbContext context) : IOrderRepository
{
    private readonly GamestoreDbContext _context = context;

    public async Task<Guid> AddAsync(Order entity)
    {
        await _context.Orders.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<bool> OrderExistsAsync(Guid id)
    {
        return await _context.Orders.AnyAsync(g => g.Id == id);
    }

    public async Task<IEnumerable<Order>> GetPaidAndCancelledOrdersAsync()
    {
        return await _context.Orders
       .Where(o => o.Status == Domain.Enums.OrderStatus.Paid || o.Status == Domain.Enums.OrderStatus.Cancelled)
       .ToListAsync();
    }

    public async Task<Order> GetOrderByIdAsync(Guid id)
    {
        return await _context.Orders.FindAsync(id);
    }

    public async Task<Guid?> GetActiveOrderIdByCustomerIdAsync(Guid customerId)
    {
        return await _context.Orders
            .Where(g => g.CustomerId == customerId && g.Status == Domain.Enums.OrderStatus.Open)
            .Select(o => (Guid?)o.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> IsOrderEmptyAsync(Guid orderId)
    {
        return !await _context.OrderGames
            .AnyAsync(og => og.OrderId == orderId);
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var exist = await _context.Orders.FindAsync(id);
        if (exist != null)
        {
            _ = _context.Orders.Remove(exist);
            _ = await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task UpdateAsync(Order entity)
    {
        _context.Orders.Update(entity);
        await _context.SaveChangesAsync();
    }
}
