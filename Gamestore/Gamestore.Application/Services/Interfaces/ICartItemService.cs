using Gamestore.Domain.Models.DTO.CartItem;

namespace Gamestore.Application.Services.Interfaces;

public interface ICartItemService
{
    Task<CartItemDto?> GetAsync(Guid orderId, Guid productId);

    Task<bool> DeleteAsync(string gameKey, Guid customerId);

    Task AddGameToCartItemAsync(string gameKey, Guid customerId);

    Task<IEnumerable<CartItemDto>> GetCartItemsByOrderIdAsync(Guid orderId);
}
