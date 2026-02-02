using Gamestore.Application.Helpers.Interfaces;
using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Enums;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models.Configuration;
using Gamestore.Domain.Models.DTO.Payment;
using Microsoft.Extensions.Options;

namespace Gamestore.Application.Services;

public class PaymentService(IOrderService orderService, IOrderRepository orderRepository, IOptions<PaymentSettings> paymentSettings, IEnumerable<IPaymentStrategy> paymentStrategies) : IPaymentService
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
        var strategy = paymentStrategies.FirstOrDefault(s => s.PaymentMethodName() == paymentRequest.Method) ?? throw new BadRequestException($"Payment method {paymentRequest.Method} is not supported.");

        var orderId = await orderService.GetOpenOrderIdAsync(customerId);
        var amount = await orderService.CalculataOrderTotalAsync(orderId);

        var order = await orderRepository.GetOrderByIdAsync(orderId);
        order.Status = OrderStatus.Checkout;
        await orderRepository.UpdateAsync(order);

        PaymentResult result;

        try
        {
            result = await strategy.ProcessPaymentAsync(customerId, orderId, amount);
        }
        catch
        {
            order.Status = OrderStatus.Open;
            await orderRepository.UpdateAsync(order);
            throw;
        }

        order.Status = result.IsSuccess ? OrderStatus.Paid : OrderStatus.Cancelled;

        await orderRepository.UpdateAsync(order);

        return result;
    }
}
