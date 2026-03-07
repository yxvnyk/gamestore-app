using Gamestore.Application.Models;
using Gamestore.Domain.Models.DTO.OrderItem;

namespace Gamestore.Application.Services.Interfaces;

public interface IOrderItemService
{
    Task<OrderItemDto?> GetAsync(Guid orderId, Guid productId);

    Task<IEnumerable<OrderItemDto>> GetCartByCustomerId(Guid customerId);

    Task DeleteAsync(string gameKey, Guid customerId);

    Task AddGameToCartAsync(string gameKey, Guid customerId);

    Task<IEnumerable<OrderItemDto>> GetOrderItemsByOrderIdAsync(Identity id);
}
