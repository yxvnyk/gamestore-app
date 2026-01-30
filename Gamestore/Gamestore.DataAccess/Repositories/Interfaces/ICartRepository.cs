using Gamestore.DataAccess.Entities;

namespace Gamestore.DataAccess.Repositories.Interfaces;

public interface ICartRepository
{
    Task<OrderGame?> GetByOrderIdProductIdAsync(Guid orderId, Guid productId);

    Task<bool> DeleteByOrderIdProductIdAsync(Guid orderId, Guid productId);

    Task UpdateGenreAsync(OrderGame entity);
}
