using System.Transactions;
using AutoMapper;
using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Enums;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models.DTO.OrderItem;
using Microsoft.Extensions.Logging;

namespace Gamestore.Application.Services;

public class OrderItemService(
    IOrderItemRepository orderItemRepository,
    IGameRepository gameRepository,
    IOrderRepository orderRepository,
    IMapper mapper,
    ILogger<OrderItemService> logger) : IOrderItemService
{
    public async Task<OrderItemDto?> GetAsync(Guid orderId, Guid productId)
    {
        var item = await orderItemRepository.GetByOrderIdProductIdAsync(orderId, productId);
        return item is not null
            ? mapper.Map<OrderItemDto>(item)
            : throw new NotFoundException("Cart item not found.");
    }

    public async Task<IEnumerable<OrderItemDto>> GetCartByCustomerId(Guid customerId)
    {
        var orderId = await GetActiveOrderIdAsync(customerId);
        if (orderId == null)
        {
            return [];
        }

        var items = await orderItemRepository.GetByOrderIdAsync(orderId.Value);
        return mapper.Map<IEnumerable<OrderItemDto>>(items);
    }

    public async Task<IEnumerable<OrderItemDto>> GetOrderItemsByOrderIdAsync(Guid orderId)
    {
        var items = await orderItemRepository.GetByOrderIdAsync(orderId);
        return mapper.Map<IEnumerable<OrderItemDto>>(items);
    }

    public async Task DeleteAsync(string gameKey, Guid customerId)
    {
        logger.LogInformation("Deleting game {GameKey} from cart for customer {CustomerId}", gameKey, customerId);

        var gameId = await GetGameIdOrThrowAsync(gameKey);
        var orderId = await GetActiveOrderIdAsync(customerId);

        if (orderId == null)
        {
            return;
        }

        using var scope = CreateTransactionScope();

        if (!await orderItemRepository.DeleteByOrderIdProductIdAsync(orderId.Value, gameId))
        {
            throw new NotFoundException($"Item not found in order {orderId}");
        }

        if (await orderRepository.IsOrderEmptyAsync(orderId.Value))
        {
            await orderRepository.DeleteByIdAsync(orderId.Value);
        }

        scope.Complete();
    }

    public async Task AddGameToCartAsync(string gameKey, Guid customerId)
    {
        logger.LogInformation("Adding game {GameKey} to cart for customer {CustomerId}", gameKey, customerId);

        var gameId = await GetGameIdOrThrowAsync(gameKey);
        var unitsInStock = await gameRepository.GetUnitsInStockAsync(gameId);

        using var scope = CreateTransactionScope();

        var orderId = await GetOrCreateActiveOrderAsync(customerId);

        var cartItem = await orderItemRepository.GetByOrderIdProductIdAsync(orderId, gameId);

        var newQuantity = CalculateCartItemQuantity(cartItem);

        if (newQuantity > unitsInStock)
        {
            throw new OrderLimitationException($"Not enough stock. Required: {newQuantity}, Available: {unitsInStock}");
        }

        if (cartItem is null)
        {
            await CreateNewCartItemAsync(orderId, gameId);
        }
        else
        {
            cartItem.Quantity = newQuantity;
            await orderItemRepository.UpdateAsync(cartItem);
        }

        scope.Complete();
    }

    private static int CalculateCartItemQuantity(OrderGame? cartItem)
    {
        return (cartItem?.Quantity ?? 0) + 1;
    }

    private async Task<Guid> GetGameIdOrThrowAsync(string gameKey)
    {
        return await gameRepository.GetGameIdByKeyAsync(gameKey)
               ?? throw new NotFoundException($"Game with key {gameKey} not found.");
    }

    private async Task<Guid?> GetActiveOrderIdAsync(Guid customerId)
    {
        return await orderRepository.GetOpenOrderIdByCustomerIdAsync(customerId);
    }

    private async Task<Guid> GetOrCreateActiveOrderAsync(Guid customerId)
    {
        var orderId = await orderRepository.GetOpenOrderIdByCustomerIdAsync(customerId);
        if (orderId != null && orderId != Guid.Empty)
        {
            return orderId.Value;
        }

        var newOrder = new Order
        {
            CustomerId = customerId,
            DateTime = DateTime.UtcNow,
            Status = OrderStatus.Open,
        };
        return await orderRepository.AddAsync(newOrder);
    }

    private async Task CreateNewCartItemAsync(Guid orderId, Guid gameId)
    {
        var game = await gameRepository.GetGameByIdAsync(gameId)
                   ?? throw new NotFoundException($"Game {gameId} not found during item creation.");

        var orderGame = new OrderGame
        {
            OrderId = orderId,
            ProductId = gameId,
            Quantity = 1,
            Discount = game.Discount,
            Price = game.Price,
        };
        await orderItemRepository.CreateAsync(orderGame);
    }

    private static TransactionScope CreateTransactionScope()
    {
        return new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);
    }
}