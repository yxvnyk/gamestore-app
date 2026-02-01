using Gamestore.DataAccess.Entities;

namespace Gamestore.DataAccess.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<Guid> AddAsync(Order entity);

    Task<bool> OrderExistsAsync(Guid id);

    Task<IEnumerable<Order>> GetPaidAndCancelledOrdersAsync();

    Task<Order> GetOrderByIdAsync(Guid id);

    Task<bool> IsOrderEmptyAsync(Guid orderId);

    Task<Guid?> GetActiveOrderIdByCustomerIdAsync(Guid customerId);

    Task<bool> DeleteByIdAsync(Guid id);

    Task UpdateAsync(Order entity);
}
