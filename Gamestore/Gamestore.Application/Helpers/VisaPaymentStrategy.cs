using Gamestore.Application.Helpers.Interfaces;
using Gamestore.Domain.Models.DTO.Payment;

namespace Gamestore.Application.Helpers;

public class VisaPaymentStrategy : IPaymentStrategy
{
    private const string PaymentMethod = "Visa";

    public string PaymentMethodName()
    {
        return PaymentMethod;
    }

    public async Task<PaymentResult> ProcessPaymentAsync(Guid customerId, Guid orderId, double amount)
    {
        return await Task.FromResult(PaymentResult.Success());
    }
}
