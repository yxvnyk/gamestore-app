using Gamestore.Application.Services.Interfaces;
using Gamestore.Application.Services.Interfaces.Payments;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Enums;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models.Configuration;
using Gamestore.Domain.Models.DTO.Payment;
using Gamestore.Domain.Models.DTO.Payment.Strategy;
using Microsoft.Extensions.Options;

namespace Gamestore.Application.Services.Payments;

public class PaymentService(IOrderService orderService, IOrderItemRepository orderItemRepository,
    IInventoryService gameStockManagerService, IOrderRepository orderRepository, IOptions<PaymentSettings> paymentSettings, IEnumerable<IPaymentStrategy> paymentStrategies) : IPaymentService
{
    private readonly PaymentSettings _paymentSettings = paymentSettings.Value;

    public PaymentMethodsWrapper GetPaymentMethods()
    {
        return new PaymentMethodsWrapper()
        {
            PaymentMethods = _paymentSettings.Methods,
        };
    }

    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequestDto paymentRequest, Guid customerId)
    {
        var strategy = GetStrategyOrThrow(paymentRequest.Method);
        var orderId = await orderService.GetOpenOrderIdAsync(customerId);
        var order = await orderRepository.GetOrderByIdAsync(orderId);
        var orderItems = await orderItemRepository.GetByOrderIdAsync(orderId);

        if (!orderItems.Any())
        {
            throw new BadRequestException("Cannot checkout an empty order.");
        }

        await gameStockManagerService.ReserveStockGameForOrderAsync(orderItems);

        await orderService.UpdateOrderStatusAsync(order, OrderStatus.Checkout);
        var amount = await orderService.CalculataOrderTotalAsync(orderId);
        PaymentContextDto simplePayDto = new()
        {
            CustomerId = customerId,
            OrderId = orderId,
            Amount = amount,
            VisaModel = paymentRequest.Model,
        };

        PaymentResult result;
        try
        {
            result = await strategy.ProcessPaymentAsync(simplePayDto);
        }
        catch (Exception)
        {
            await RollbackOrderAsync(order, orderItems);
            throw;
        }

        if (result.IsSuccess)
        {
            await orderService.UpdateOrderStatusAsync(order, OrderStatus.Paid);
        }
        else
        {
            await RollbackOrderAsync(order, orderItems);
        }

        return result;
    }

    private IPaymentStrategy GetStrategyOrThrow(string methodName)
    {
        return paymentStrategies.FirstOrDefault(s => s.PaymentMethodName() == methodName)
               ?? throw new BadRequestException($"Payment method {methodName} is not supported.");
    }

    private async Task RollbackOrderAsync(Order order, IEnumerable<OrderGame> items)
    {
        await gameStockManagerService.ReserveStockGameForOrderAsync(items);
        await orderService.UpdateOrderStatusAsync(order, OrderStatus.Open);
    }
}
