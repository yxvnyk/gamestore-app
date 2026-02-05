using Gamestore.Application.Helpers.Interfaces;
using Gamestore.Domain.Models.DTO.Payment;
using Gamestore.Domain.Models.DTO.Payment.Provider;
using Gamestore.Infrastructure.ExternalServices;

namespace Gamestore.Application.Helpers;

public class IBoxPaymentStrategy(IPaymentProxy paymentProxy) : IPaymentStrategy
{
    private const string PaymentMethod = "IBox terminal";

    public string PaymentMethodName()
    {
        return PaymentMethod;
    }

    public async Task<PaymentResult> ProcessPaymentAsync(Guid customerId, Guid orderId, double amount)
    {
        var creationDate = DateTime.UtcNow;
        var dto = new IBoxPayRequestDto
        {
            AccountNumber = customerId,
            InvoiceNumber = orderId,
            TransactionAmount = amount,
        };

        await paymentProxy.PayIBoxAsync(dto);
        IBoxPaymentDataDto data = new()
        {
            UserId = customerId,
            OrderId = orderId,
            PaymentData = creationDate,
            Sum = amount,
        };

        return await Task.FromResult(PaymentResult.SuccessWithData(data));
    }
}
