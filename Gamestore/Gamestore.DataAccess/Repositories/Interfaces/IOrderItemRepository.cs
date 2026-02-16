using Gamestore.DataAccess.Entities;

namespace Gamestore.DataAccess.Repositories.Interfaces;

public interface IOrderItemRepository
{
    Task<OrderGame?> GetByOrderIdProductIdAsync(Guid orderId, Guid productId);

    Task CreateAsync(OrderGame entity);

    Task<IEnumerable<OrderGame>> GetByOrderIdAsync(Guid orderId);

    Task<bool> DeleteByOrderIdProductIdAsync(Guid orderId, Guid productId);

    Task UpdateAsync(OrderGame entity);
}
