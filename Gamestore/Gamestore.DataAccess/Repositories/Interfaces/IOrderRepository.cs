using Gamestore.DataAccess.Entities;

namespace Gamestore.DataAccess.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<bool> OrderExistsAsync(Guid id);

    Task<IEnumerable<Order>> GetPaidAndCancelledOrdersAsync();

    Task<Order> GetOrderByIdAsync(Guid id);

    Task<bool> DeleteByIdAsync(Guid id);

    Task UpdateGenreAsync(Order entity);
}
