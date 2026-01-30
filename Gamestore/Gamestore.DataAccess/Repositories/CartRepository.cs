using Gamestore.DataAccess.Context;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DataAccess.Repositories;

public class CartRepository(GamestoreDbContext context) : ICartRepository
{
    private readonly GamestoreDbContext _context = context;

    public async Task<OrderGame?> GetByOrderIdProductIdAsync(Guid orderId, Guid productId)
    {
        return await _context.OrderGames
            .Where(og => og.OrderId == orderId && og.ProductId == productId)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> DeleteByOrderIdProductIdAsync(Guid orderId, Guid productId)
    {
        var exist = await GetByOrderIdProductIdAsync(orderId, productId);
        if (exist != null)
        {
            _ = _context.OrderGames.Remove(exist);
            _ = await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task UpdateGenreAsync(OrderGame entity)
    {
        _context.OrderGames.Update(entity);
        await _context.SaveChangesAsync();
    }
}
