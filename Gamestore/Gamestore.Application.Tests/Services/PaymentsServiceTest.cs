using Gamestore.Application.Services.Interfaces;
using Gamestore.Application.Services.Interfaces.Payments;
using Gamestore.Application.Services.Payments;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Enums;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models.Configuration;
using Gamestore.Domain.Models.DTO.Payment;
using Gamestore.Domain.Models.DTO.Payment.Strategy;
using Microsoft.Extensions.Options;
using Moq;

namespace Gamestore.Application.Tests.Services;

public class PaymentsServiceTest
{
    private readonly Mock<IOrderService> _mockOrderService = new();
    private readonly Mock<IOrderItemRepository> _mockOrderItemRepo = new();
    private readonly Mock<IInventoryService> _mockInventory = new();
    private readonly Mock<IOrderRepository> _mockOrderRepo = new();
    private readonly Mock<IPaymentStrategy> _mockStrategy = new();

    [Fact]
    public void GetPaymentMethods_ReturnsSettings()
    {
        // Arrange
        var method = new PaymentMethodDto
        {
            ImageUrl = new Uri("https://example.test/method.png"),
            Title = "Visa",
            Description = "visa",
        };
        var settings = new PaymentSettings { Methods = [method] };
        var svc = CreateService(settings);

        // Act
        var wrapper = svc.GetPaymentMethods();

        // Assert
        Assert.NotNull(wrapper);
        Assert.Single(wrapper.PaymentMethods);
        Assert.Equal("Visa", wrapper.PaymentMethods[0].Title);
    }

    [Fact]
    public async Task ProcessPaymentAsync_Success_UpdatesStatusToPaid()
    {
        // Arrange
        var svc = CreateService();
        var customerId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var order = new Order { Id = orderId, CustomerId = customerId, Status = OrderStatus.Open };
        var items = new List<OrderGame> { new() { OrderId = orderId, ProductId = Guid.NewGuid(), Quantity = 1, Price = 1.0 } };

        _mockOrderService.Setup(x => x.GetOpenOrderIdAsync(customerId)).ReturnsAsync(orderId);
        _mockOrderRepo.Setup(x => x.GetOrderByIdAsync(orderId)).ReturnsAsync(order);
        _mockOrderItemRepo.Setup(x => x.GetByOrderIdAsync(orderId)).ReturnsAsync(items);
        _mockInventory.Setup(x => x.ReserveStockGameForOrderAsync(items)).Returns(Task.CompletedTask);
        _mockOrderService.Setup(x => x.CalculataOrderTotalAsync(orderId)).ReturnsAsync(123.45);
        _mockStrategy.Setup(s => s.PaymentMethodName()).Returns("visa");
        _mockStrategy.Setup(s => s.ProcessPaymentAsync(It.IsAny<PaymentContextDto>())).ReturnsAsync(PaymentResult.Success());

        var svcWithStrategy = CreateService(null, [_mockStrategy.Object]);

        // Act
        var req = new PaymentRequestDto { Method = "visa", Model = null };
        var result = await svcWithStrategy.ProcessPaymentAsync(req, customerId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        _mockInventory.Verify(x => x.ReserveStockGameForOrderAsync(items), Times.Once);
        _mockOrderService.Verify(x => x.UpdateOrderStatusAsync(order, OrderStatus.Checkout), Times.Once);
        _mockOrderService.Verify(x => x.UpdateOrderStatusAsync(order, OrderStatus.Paid), Times.Once);
        _mockStrategy.Verify(s => s.ProcessPaymentAsync(It.Is<PaymentContextDto>(c => c.OrderId == orderId && Math.Abs(c.Amount - 123.45) < 0.001)), Times.Once);
    }

    [Fact]
    public async Task ProcessPaymentAsync_FailedResult_RollsBackAndSetsOpen()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var order = new Order { Id = orderId, CustomerId = customerId, Status = OrderStatus.Open };
        var items = new List<OrderGame> { new() { OrderId = orderId, ProductId = Guid.NewGuid(), Quantity = 1, Price = 1.0 } };

        _mockOrderService.Setup(x => x.GetOpenOrderIdAsync(customerId)).ReturnsAsync(orderId);
        _mockOrderRepo.Setup(x => x.GetOrderByIdAsync(orderId)).ReturnsAsync(order);
        _mockOrderItemRepo.Setup(x => x.GetByOrderIdAsync(orderId)).ReturnsAsync(items);
        _mockInventory.Setup(x => x.ReserveStockGameForOrderAsync(items)).Returns(Task.CompletedTask);
        _mockOrderService.Setup(x => x.CalculataOrderTotalAsync(orderId)).ReturnsAsync(10.0);
        _mockStrategy.Setup(s => s.PaymentMethodName()).Returns("box");
        _mockStrategy.Setup(s => s.ProcessPaymentAsync(It.IsAny<PaymentContextDto>())).ReturnsAsync(PaymentResult.Fail("declined"));

        var svc = CreateService(null, [_mockStrategy.Object]);

        // Act
        var req = new PaymentRequestDto { Method = "box", Model = null };
        var result = await svc.ProcessPaymentAsync(req, customerId);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        _mockInventory.Verify(x => x.ReserveStockGameForOrderAsync(items), Times.Exactly(2));
        _mockOrderService.Verify(x => x.UpdateOrderStatusAsync(order, OrderStatus.Checkout), Times.Once);
        _mockOrderService.Verify(x => x.UpdateOrderStatusAsync(order, OrderStatus.Open), Times.Once);
    }

    [Fact]
    public async Task ProcessPaymentAsync_StrategyThrows_RollsBackAndRethrows()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var order = new Order { Id = orderId, CustomerId = customerId, Status = OrderStatus.Open };
        OrderGame orderGame = new() { OrderId = orderId, ProductId = Guid.NewGuid(), Quantity = 1, Price = 1.0 };
        var items = new List<OrderGame> { orderGame };

        _mockOrderService.Setup(x => x.GetOpenOrderIdAsync(customerId)).ReturnsAsync(orderId);
        _mockOrderRepo.Setup(x => x.GetOrderByIdAsync(orderId)).ReturnsAsync(order);
        _mockOrderItemRepo.Setup(x => x.GetByOrderIdAsync(orderId)).ReturnsAsync(items);
        _mockInventory.Setup(x => x.ReserveStockGameForOrderAsync(items)).Returns(Task.CompletedTask);
        _mockOrderService.Setup(x => x.CalculataOrderTotalAsync(orderId)).ReturnsAsync(10.0);
        _mockStrategy.Setup(s => s.PaymentMethodName()).Returns("visa");
        _mockStrategy.Setup(s => s.ProcessPaymentAsync(It.IsAny<PaymentContextDto>())).ThrowsAsync(new InvalidOperationException("boom"));

        var svc = CreateService(null, [_mockStrategy.Object]);

        // Act & Assert
        var req = new PaymentRequestDto { Method = "visa", Model = null };
        await Assert.ThrowsAsync<InvalidOperationException>(() => svc.ProcessPaymentAsync(req, customerId));
        _mockInventory.Verify(x => x.ReserveStockGameForOrderAsync(items), Times.Exactly(2));
        _mockOrderService.Verify(x => x.UpdateOrderStatusAsync(order, OrderStatus.Open), Times.Once);
    }

    [Fact]
    public async Task ProcessPaymentAsync_EmptyOrder_ThrowsBadRequestException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var order = new Order { Id = orderId, CustomerId = customerId, Status = OrderStatus.Open };

        _mockOrderService.Setup(x => x.GetOpenOrderIdAsync(customerId)).ReturnsAsync(orderId);
        _mockOrderRepo.Setup(x => x.GetOrderByIdAsync(orderId)).ReturnsAsync(order);
        _mockOrderItemRepo.Setup(x => x.GetByOrderIdAsync(orderId)).ReturnsAsync([]);

        var svc = CreateService(null, [_mockStrategy.Object]);

        // Act & Assert
        var req = new PaymentRequestDto { Method = "visa", Model = null };
        await Assert.ThrowsAsync<BadRequestException>(() => svc.ProcessPaymentAsync(req, customerId));

        _mockInventory.Verify(x => x.ReserveStockGameForOrderAsync(It.IsAny<IEnumerable<OrderGame>>()), Times.Never);
        _mockStrategy.Verify(s => s.ProcessPaymentAsync(It.IsAny<PaymentContextDto>()), Times.Never);
    }

    private PaymentService CreateService(PaymentSettings? settings = null, IEnumerable<IPaymentStrategy>? strategies = null)
    {
        var opts = Options.Create(settings ?? new PaymentSettings { Methods = [] });
        return new PaymentService(
            _mockOrderService.Object,
            _mockOrderItemRepo.Object,
            _mockInventory.Object,
            _mockOrderRepo.Object,
            opts,
            strategies ?? [_mockStrategy.Object]);
    }
}