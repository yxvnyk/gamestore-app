using AutoMapper;
using Gamestore.Application.Services;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models.DTO.OrderItem;
using Microsoft.Extensions.Logging;
using Moq;

namespace Gamestore.Application.Tests.Services;

public class OrderItemServiceTest
{
    private readonly Mock<IOrderItemRepository> _mockOrderItemRepo = new();
    private readonly Mock<IGameRepository> _mockGameRepo = new();
    private readonly Mock<IOrderRepository> _mockOrderRepo = new();
    private readonly Mock<ILogger<OrderItemService>> _mockLogger = new();
    private readonly Mock<IMapper> _mockMapper = new();

    [Fact]
    public async Task AddGameToCart_NewItem_CreatesItem()
    {
        // Arrange
        var service = CreateService();
        var gameKey = "game-key";
        var customerId = Guid.NewGuid();
        var gameId = Guid.NewGuid();
        var createdOrderId = Guid.NewGuid();

        _mockGameRepo.Setup(r => r.GetGameIdByKeyAsync(gameKey)).ReturnsAsync(gameId);
        _mockGameRepo.Setup(r => r.GetUnitsInStockAsync(gameId)).ReturnsAsync(10);
        _mockOrderRepo.Setup(r => r.GetOpenOrderIdByCustomerIdAsync(customerId)).ReturnsAsync((Guid?)null);
        _mockOrderRepo.Setup(r => r.AddAsync(It.IsAny<Order>())).ReturnsAsync(createdOrderId);
        _mockOrderItemRepo.Setup(r => r.GetByOrderIdProductIdAsync(createdOrderId, gameId)).ReturnsAsync((OrderGame?)null);
        _mockGameRepo.Setup(r => r.GetByIdAsync(gameId)).ReturnsAsync(new Game { Id = gameId, Price = 5.5, Discount = 1 });

        // Act
        await service.AddGameToCartAsync(gameKey, customerId);

        // Assert
        _mockOrderItemRepo.Verify(
            r => r.CreateAsync(It.Is<OrderGame>(og =>
            og.OrderId == createdOrderId &&
            og.ProductId == gameId &&
            og.Quantity == 1 &&
            Math.Abs(og.Price - 5.5) < 0.0001 &&
            og.Discount == 1)),
            Times.Once());
    }

    [Fact]
    public async Task AddGameToCart_ExistingItem_IncrementsQuantity()
    {
        // Arrange
        var service = CreateService();
        var gameKey = "game-key";
        var customerId = Guid.NewGuid();
        var gameId = Guid.NewGuid();
        var orderId = Guid.NewGuid();

        _mockGameRepo.Setup(r => r.GetGameIdByKeyAsync(gameKey)).ReturnsAsync(gameId);
        _mockGameRepo.Setup(r => r.GetUnitsInStockAsync(gameId)).ReturnsAsync(10);
        _mockOrderRepo.Setup(r => r.GetOpenOrderIdByCustomerIdAsync(customerId)).ReturnsAsync(orderId);

        var existing = new OrderGame
        {
            OrderId = orderId,
            ProductId = gameId,
            Quantity = 1,
            Price = 3.3,
            Discount = 0,
        };
        _mockOrderItemRepo.Setup(r => r.GetByOrderIdProductIdAsync(orderId, gameId)).ReturnsAsync(existing);

        // Act
        await service.AddGameToCartAsync(gameKey, customerId);

        // Assert
        _mockOrderItemRepo.Verify(r => r.UpdateAsync(It.Is<OrderGame>(og => og.OrderId == orderId && og.ProductId == gameId && og.Quantity == 2)), Times.Once);
    }

    [Fact]
    public async Task AddGameToCart_NotEnoughStock_ThrowsOrderLimitationException()
    {
        // Arrange
        var service = CreateService();
        var gameKey = "game-key";
        var customerId = Guid.NewGuid();
        var gameId = Guid.NewGuid();
        var orderId = Guid.NewGuid();

        _mockGameRepo.Setup(r => r.GetGameIdByKeyAsync(gameKey)).ReturnsAsync(gameId);
        _mockGameRepo.Setup(r => r.GetUnitsInStockAsync(gameId)).ReturnsAsync(1);
        _mockOrderRepo.Setup(r => r.GetOpenOrderIdByCustomerIdAsync(customerId)).ReturnsAsync(orderId);

        var existing = new OrderGame
        {
            OrderId = orderId,
            ProductId = gameId,
            Quantity = 1,
            Price = 3.3,
            Discount = 0,
        };
        _mockOrderItemRepo.Setup(r => r.GetByOrderIdProductIdAsync(orderId, gameId)).ReturnsAsync(existing);

        // Act & Assert
        await Assert.ThrowsAsync<OrderLimitationException>(() => service.AddGameToCartAsync(gameKey, customerId));

        // ensure no update/create called
        _mockOrderItemRepo.Verify(r => r.UpdateAsync(It.IsAny<OrderGame>()), Times.Never);
        _mockOrderItemRepo.Verify(r => r.CreateAsync(It.IsAny<OrderGame>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_NoOrder_DoesNotCallDelete()
    {
        // Arrange
        var service = CreateService();
        var gameKey = "game-key";
        var customerId = Guid.NewGuid();

        _mockGameRepo.Setup(r => r.GetGameIdByKeyAsync(gameKey)).ReturnsAsync(Guid.NewGuid());
        _mockOrderRepo.Setup(r => r.GetOpenOrderIdByCustomerIdAsync(customerId)).ReturnsAsync((Guid?)null);

        // Act
        await service.DeleteAsync(gameKey, customerId);

        // Assert
        _mockOrderItemRepo.Verify(r => r.DeleteByOrderIdProductIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ItemRemovedAndOrderDeleted()
    {
        // Arrange
        var service = CreateService();
        var gameKey = "game-key";
        var customerId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var gameId = Guid.NewGuid();

        _mockGameRepo.Setup(r => r.GetGameIdByKeyAsync(gameKey)).ReturnsAsync(gameId);
        _mockOrderRepo.Setup(r => r.GetOpenOrderIdByCustomerIdAsync(customerId)).ReturnsAsync(orderId);
        _mockOrderItemRepo.Setup(r => r.DeleteByOrderIdProductIdAsync(orderId, gameId)).ReturnsAsync(true);
        _mockOrderRepo.Setup(r => r.IsOrderEmptyAsync(orderId)).ReturnsAsync(true);
        _mockOrderRepo.Setup(r => r.DeleteByIdAsync(orderId)).ReturnsAsync(true);

        // Act
        await service.DeleteAsync(gameKey, customerId);

        // Assert
        _mockOrderItemRepo.Verify(r => r.DeleteByOrderIdProductIdAsync(orderId, gameId), Times.Once);
        _mockOrderRepo.Verify(r => r.DeleteByIdAsync(orderId), Times.Once);
    }

    [Fact]
    public async Task GetAsync_ReturnsMappedDto()
    {
        // Arrange
        var service = CreateService();
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var orderGame = new OrderGame
        {
            OrderId = orderId,
            ProductId = productId,
            Price = 7.7,
            Quantity = 3,
            Discount = 2,
        };

        _mockOrderItemRepo.Setup(r => r.GetByOrderIdProductIdAsync(orderId, productId)).ReturnsAsync(orderGame);
        _mockMapper.Setup(m => m.Map<OrderItemDto>(It.IsAny<OrderGame>())).Returns((OrderGame og) => new OrderItemDto
        {
            ProductId = og.ProductId,
            Price = og.Price,
            Quantity = og.Quantity,
            Discount = og.Discount,
        });

        // Act
        var dto = await service.GetAsync(orderId, productId);

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(productId, dto!.ProductId);
        Assert.Equal(7.7, dto.Price);
        Assert.Equal(3, dto.Quantity);
        Assert.Equal(2, dto.Discount);
    }

    [Fact]
    public async Task GetCartByCustomerId_NoOrder_ReturnsEmpty()
    {
        // Arrange
        var service = CreateService();
        var customerId = Guid.NewGuid();

        _mockOrderRepo.Setup(r => r.GetOpenOrderIdByCustomerIdAsync(customerId)).ReturnsAsync((Guid?)null);

        // Act
        var result = await service.GetCartByCustomerId(customerId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    private OrderItemService CreateService() => new(
        _mockOrderItemRepo.Object,
        _mockGameRepo.Object,
        _mockOrderRepo.Object,
        _mockMapper.Object,
        _mockLogger.Object);
}