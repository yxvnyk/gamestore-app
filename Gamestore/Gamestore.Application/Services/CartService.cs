using AutoMapper;
using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models.DTO.Cart;
using Microsoft.Extensions.Logging;

namespace Gamestore.Application.Services;

public class CartService(ICartRepository cartRepository, IGameRepository gameRepository, IOrderRepository orderRepository, IMapper mapper, ILogger<CartService> logger) : ICartService
{
    public async Task<CartDto?> GetAsync(Guid orderId, Guid productId)
    {
        logger.LogInformation(nameof(this.GetAsync));

        var order = await cartRepository.GetByOrderIdProductIdAsync(orderId, productId);
        return order is not null ? mapper.Map<CartDto>(order) : throw new NotFoundException($"Cart does not exist.");
    }

    public async Task<bool> DeleteAsync(string gameKey, Guid customerId)
    {
        logger.LogInformation(nameof(this.GetAsync));

        var gameId = await gameRepository.GetGameIdByKeyAsync(gameKey) ?? throw new NotFoundException($"Game with key {gameKey} not found.");

        var orderId = await orderRepository.GetOrderIdByCustomerIdAsync(customerId) ?? throw new NotFoundException($"Active order for customer ID {customerId} not found.");

        return await cartRepository.DeleteByOrderIdProductIdAsync(orderId, gameId);
    }

    public async Task AddGameToCartAsync(string gameKey, Guid customerId)
    {
        logger.LogInformation(nameof(this.AddGameToCartAsync));

        var gameId = await gameRepository.GetGameIdByKeyAsync(gameKey) ?? throw new NotFoundException($"Game with key {gameKey} not found.");

        var orderId = await orderRepository.GetOrderIdByCustomerIdAsync(customerId) ?? throw new NotFoundException($"Active order for customer ID {customerId} not found.");

        var cart = await cartRepository.GetByOrderIdProductIdAsync(orderId, gameId) ?? throw new NotFoundException($"Cart with order ID {orderId}, and game ID {gameId} does not exist.");

        var unitInStock = await gameRepository.GetUnitsInStockAsync(gameId);
        if (cart.Quantity + 1 > unitInStock)
        {
            throw new OrderLimitationException();
        }

        cart.Quantity += 1;

        await cartRepository.UpdateGenreAsync(cart);
    }
}
