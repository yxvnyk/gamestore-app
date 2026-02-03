using Gamestore.Application.Helpers.Interfaces;
using Gamestore.Domain.Models.DTO.Payment;

namespace Gamestore.Application.Helpers;

public class IBoxPaymentStrategy : IPaymentStrategy
{
    private const string PaymentMethod = "IBox terminal";

    public string PaymentMethodName()
    {
        return PaymentMethod;
    }

    public async Task<PaymentResult> ProcessPaymentAsync(Guid customerId, Guid orderId, double amount)
    {
        var creationDate = DateTime.UtcNow;

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
