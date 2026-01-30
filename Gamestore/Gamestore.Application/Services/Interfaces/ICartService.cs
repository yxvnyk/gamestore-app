using Gamestore.Domain.Models.DTO.Cart;

namespace Gamestore.Application.Services.Interfaces;

public interface ICartService
{
    Task<CartDto?> GetAsync(Guid orderId, Guid productId);

    Task<bool> DeleteAsync(string gameKey, Guid customerId);

    Task AddGameToCartAsync(string gameKey, Guid customerId);
}
