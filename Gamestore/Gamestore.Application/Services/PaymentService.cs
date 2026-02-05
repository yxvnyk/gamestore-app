using System.Transactions;
using Gamestore.Application.Helpers.Interfaces;
using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Enums;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models.Configuration;
using Gamestore.Domain.Models.DTO.Payment;
using Gamestore.Domain.Models.DTO.Payment.Strategy;
using Microsoft.Extensions.Options;

namespace Gamestore.Application.Services;

public class PaymentService(IOrderService orderService, IOrderItemRepository orderItemRepository,
    IGameRepository gameRepository, IOrderRepository orderRepository, IOptions<PaymentSettings> paymentSettings, IEnumerable<IPaymentStrategy> paymentStrategies) : IPaymentService
{
    private readonly PaymentSettings _paymentSettings = paymentSettings.Value;

    public PaymentMethodsResponse GetPaymentMethods()
    {
        return new PaymentMethodsResponse()
        {
            PaymentMethods = _paymentSettings.Methods,
        };
    }

    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequestDto paymentRequest, Guid customerId)
    {
        var strategy = GetPaymentStrategyOrThrow(paymentRequest.Method);
        var orderId = await orderService.GetOpenOrderIdAsync(customerId);
        var order = await orderRepository.GetOrderByIdAsync(orderId);
        var orderItems = await orderItemRepository.GetByOrderIdAsync(orderId);

        if (!orderItems.Any())
        {
            throw new BadRequestException("Cannot checkout an empty order.");
        }

        await ReserveStockForOrderAsync(orderItems);

        await UpdateOrderStatusAsync(order, OrderStatus.Checkout);

        PaymentResult result;
        try
        {
            var amount = await orderService.CalculataOrderTotalAsync(orderId);
            SimplePayDto simplePayDto = new()
            {
                CustomerId = customerId,
                OrderId = orderId,
                Amount = amount,
                VisaDetails = paymentRequest.Model,
            };
            result = await strategy.ProcessPaymentAsync(simplePayDto);
        }
        catch (Exception)
        {
            await HandleFailedPaymentAsync(order, orderItems);
            throw;
        }

        if (result.IsSuccess)
        {
            await UpdateOrderStatusAsync(order, OrderStatus.Paid);
        }
        else
        {
            await HandleFailedPaymentAsync(order, orderItems);
        }

        return result;
    }

    private IPaymentStrategy GetPaymentStrategyOrThrow(string methodName)
    {
        return paymentStrategies.FirstOrDefault(s => s.PaymentMethodName() == methodName)
               ?? throw new BadRequestException($"Payment method {methodName} is not supported.");
    }

    private async Task ReserveStockForOrderAsync(IEnumerable<OrderGame> items)
    {
        using var scope = CreateTransactionScope();

        foreach (var item in items)
        {
            var game = await gameRepository.GetGameByIdAsync(item.ProductId);

            if (game.UnitInStock < item.Quantity)
            {
                throw new OrderLimitationException($"Not enough stock for '{game.Key}'. Required: {item.Quantity}, Available: {game.UnitInStock}");
            }

            game.UnitInStock -= item.Quantity;
            await gameRepository.UpdateGameAsync(game);
        }

        scope.Complete();
    }

    private async Task RestoreStockForOrderAsync(IEnumerable<OrderGame> items)
    {
        using var scope = CreateTransactionScope();

        foreach (var item in items)
        {
            var game = await gameRepository.GetGameByIdAsync(item.ProductId);
            game.UnitInStock += item.Quantity;
            await gameRepository.UpdateGameAsync(game);
        }

        scope.Complete();
    }

    private async Task UpdateOrderStatusAsync(Order order, OrderStatus status)
    {
        order.Status = status;
        await orderRepository.UpdateAsync(order);
    }

    private async Task HandleFailedPaymentAsync(Order order, IEnumerable<OrderGame> items)
    {
        await RestoreStockForOrderAsync(items);
        await UpdateOrderStatusAsync(order, OrderStatus.Open);
    }

    private static TransactionScope CreateTransactionScope()
    {
        return new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);
    }
}
