using Gamestore.Application.Services.Interfaces.Payments;
using Gamestore.Domain.Models.DTO.Payment;
using Gamestore.Domain.Models.DTO.Payment.Strategy;
using Gamestore.Domain.Models.DTO.Payment.Transaction;
using Gamestore.Infrastructure.ExternalServices;

namespace Gamestore.Application.Services.Payments.Strategies;

public class BoxPaymentStrategy(IPaymentProxy paymentProxy) : IPaymentStrategy
{
    private const string PaymentMethod = "IBox terminal";

    public string PaymentMethodName()
    {
        return PaymentMethod;
    }

    public async Task<PaymentResult> ProcessPaymentAsync(PaymentContextDto context)
    {
        var creationDate = DateTime.UtcNow;
        var dto = new BoxTransactionRequest
        {
            AccountNumber = context.CustomerId,
            InvoiceNumber = context.OrderId,
            TransactionAmount = context.Amount,
        };

        await paymentProxy.PayIBoxAsync(dto);
        BoxPaymenResult data = new()
        {
            UserId = context.CustomerId,
            OrderId = context.OrderId,
            PaymentDate = creationDate,
            Sum = context.Amount,
        };

        return PaymentResult.SuccessWithData(data);
    }
}
