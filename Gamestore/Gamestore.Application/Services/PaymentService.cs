using System.Transactions;
using Gamestore.Application.Helpers.Interfaces;
using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Enums;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models.Configuration;
using Gamestore.Domain.Models.DTO.Payment;
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
        // 1. Подготовка данных
        var strategy = GetPaymentStrategyOrThrow(paymentRequest.Method);
        var orderId = await orderService.GetOpenOrderIdAsync(customerId);
        var order = await orderRepository.GetOrderByIdAsync(orderId);
        var orderItems = await orderItemRepository.GetByOrderIdAsync(orderId);

        if (!orderItems.Any())
        {
            throw new BadRequestException("Cannot checkout an empty order.");
        }

        // 2. Резервирование товаров (Списание со склада)
        // Это делается в транзакции БД
        await ReserveStockForOrderAsync(orderItems);

        // Обновляем статус, что мы начали процесс
        await UpdateOrderStatusAsync(order, OrderStatus.Checkout);

        PaymentResult result;
        try
        {
            // 3. Проведение оплаты (Внешний сервис)
            // ВАЖНО: Выполняется БЕЗ транзакции БД, чтобы не блокировать таблицы
            var amount = await orderService.CalculataOrderTotalAsync(orderId);
            result = await strategy.ProcessPaymentAsync(customerId, orderId, amount);
        }
        catch (Exception)
        {
            // Ошибка сети или кода - полная отмена
            await HandleFailedPaymentAsync(order, orderItems);
            throw;
        }

        // 4. Финализация
        if (result.IsSuccess)
        {
            await UpdateOrderStatusAsync(order, OrderStatus.Paid);
        }
        else
        {
            // Оплата отклонена банком - возвращаем товары
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

    // Метод "Компенсации" (Rollback)
    private async Task HandleFailedPaymentAsync(Order order, IEnumerable<OrderGame> items)
    {
        await RestoreStockForOrderAsync(items);
        await UpdateOrderStatusAsync(order, OrderStatus.Open); // Или Cancelled, по желанию
    }

    private static TransactionScope CreateTransactionScope()
    {
        return new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);
    }
}
