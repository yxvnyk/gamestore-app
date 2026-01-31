using AutoMapper;
using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models.DTO.CartItem;
using Microsoft.Extensions.Logging;

namespace Gamestore.Application.Services;

public class CartItemService(ICartItemRepository cartRepository, IGameRepository gameRepository, IOrderRepository orderRepository, IMapper mapper, ILogger<CartItemService> logger) : ICartItemService
{
    public async Task<CartItemDto?> GetAsync(Guid orderId, Guid productId)
    {
        logger.LogInformation(nameof(this.GetAsync));

        var order = await cartRepository.GetByOrderIdProductIdAsync(orderId, productId);
        return order is not null ? mapper.Map<CartItemDto>(order) : throw new NotFoundException($"Cart does not exist.");
    }

    public async Task<IEnumerable<CartItemDto>> GetCartItemsByOrderIdAsync(Guid orderId)
    {
        logger.LogInformation(nameof(this.GetCartItemsByOrderIdAsync));
        var cartItems = await cartRepository.GetByOrderIdAsync(orderId);
        return [.. cartItems.Select(mapper.Map<CartItemDto>)];
    }

    public async Task<bool> DeleteAsync(string gameKey, Guid customerId)
    {
        logger.LogInformation(nameof(this.GetAsync));

        var gameId = await gameRepository.GetGameIdByKeyAsync(gameKey) ?? throw new NotFoundException($"Game with key {gameKey} not found.");

        var orderId = await orderRepository.GetActiveOrderIdByCustomerIdAsync(customerId) ?? throw new NotFoundException($"Active order for customer ID {customerId} not found.");

        var cartItem = await cartRepository.GetByOrderIdProductIdAsync(orderId, gameId) ?? throw new NotFoundException($"Cart item with game ID {gameId} not found.");

        if (cartItem.Quantity - 1 < 1)
        {
            return await cartRepository.DeleteByOrderIdProductIdAsync(orderId, gameId);
        }

        cartItem.Quantity -= 1;

        await cartRepository.UpdateGenreAsync(cartItem);
        return true;
    }

    public async Task AddGameToCartItemAsync(string gameKey, Guid customerId)
    {
        logger.LogInformation(nameof(this.AddGameToCartItemAsync));

        var gameId = await gameRepository.GetGameIdByKeyAsync(gameKey) ?? throw new NotFoundException($"Game with key {gameKey} not found.");

        var orderId = await orderRepository.GetActiveOrderIdByCustomerIdAsync(customerId) ?? throw new NotFoundException($"Active order for customer ID {customerId} not found.");

        var cartItem = await cartRepository.GetByOrderIdProductIdAsync(orderId, gameId);
        if (cartItem == null)
        {
            await CreateCartItemAsync(orderId, gameId);
            return;
        }

        var unitInStock = await gameRepository.GetUnitsInStockAsync(gameId);
        if (cartItem.Quantity + 1 > unitInStock)
        {
            throw new OrderLimitationException();
        }

        cartItem.Quantity += 1;

        await cartRepository.UpdateGenreAsync(cartItem);
    }

    private async Task CreateCartItemAsync(Guid orderId, Guid gameId)
    {
        logger.LogInformation(nameof(this.CreateCartItemAsync));

        var game = await gameRepository.GetGameByIdAsync(gameId) ?? throw new NotFoundException($"Game with ID {gameId} not found.");

        var orderGame = new OrderGame
        {
            OrderId = orderId,
            ProductId = gameId,
            Quantity = 1,
            Discount = game.Discount,
            Price = game.Price,
        };
        await cartRepository.CreateAsync(orderGame);
    }
}
