using Gamestore.Application.Helpers.Interfaces;
using Gamestore.Domain.Models.DTO.Payment;
using Gamestore.Domain.Models.DTO.Payment.Provider;
using Gamestore.Domain.Models.DTO.Payment.Strategy;
using Gamestore.Infrastructure.ExternalServices;

namespace Gamestore.Application.Helpers;

public class IBoxPaymentStrategy(IPaymentProxy paymentProxy) : IPaymentStrategy
{
    private const string PaymentMethod = "IBox terminal";

    public string PaymentMethodName()
    {
        return PaymentMethod;
    }

    public async Task<PaymentResult> ProcessPaymentAsync(SimplePayDto payDto)
    {
        var creationDate = DateTime.UtcNow;
        var dto = new IBoxTransactionRequest
        {
            AccountNumber = payDto.CustomerId,
            InvoiceNumber = payDto.OrderId,
            TransactionAmount = payDto.Amount,
        };

        await paymentProxy.PayIBoxAsync(dto);
        IBoxPaymentDataDto data = new()
        {
            UserId = payDto.CustomerId,
            OrderId = payDto.OrderId,
            PaymentData = creationDate,
            Sum = payDto.Amount,
        };

        return await Task.FromResult(PaymentResult.SuccessWithData(data));
    }
}
