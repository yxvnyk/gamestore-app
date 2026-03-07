using Gamestore.Application.Models;
using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Enums;
using Gamestore.Domain.Models.DTO.Order;

namespace Gamestore.Application.Services.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetPaidAndCancelledOrdersAsync();

    Task<OrderDto> GetOrderByIdAsync(Identity id);

    Task<double> CalculataOrderTotalAsync(Guid orderId);

    Task<Guid> GetOpenOrderIdAsync(Guid customerId);

    Task UpdateOrderStatusAsync(Order order, OrderStatus status);

    Task<bool> DeleteByIdAsync(Guid id);

    Task<IEnumerable<OrderDto>> GetOrderHistoryAsync();

    Task UpdateAsync(Order entity);
}
