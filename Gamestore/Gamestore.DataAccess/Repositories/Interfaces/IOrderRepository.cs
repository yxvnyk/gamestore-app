using Gamestore.DataAccess.Entities;

namespace Gamestore.DataAccess.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<Guid> AddAsync(Order entity);

    Task<bool> OrderExistsAsync(Guid id);

    Task<IEnumerable<Order>> GetPaidAndCancelledOrdersAsync();

    Task<Order> GetOrderByIdAsync(Guid id);

    Task<IEnumerable<Order>> GetAllAsync();

    Task<bool> IsOrderEmptyAsync(Guid orderId);

    Task<Guid?> GetOpenOrderIdByCustomerIdAsync(Guid customerId);

    Task<Order?> GetOpenOrderByCustomerIdAsync(Guid customerId);

    Task<bool> DeleteByIdAsync(Guid id);

    Task UpdateAsync(Order entity);
}
