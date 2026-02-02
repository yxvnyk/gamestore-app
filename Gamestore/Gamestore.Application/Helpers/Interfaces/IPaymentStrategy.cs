using Gamestore.Domain.Models.DTO.Payment;

namespace Gamestore.Application.Helpers.Interfaces;

public interface IPaymentStrategy
{
    Task<PaymentResult> ProcessPaymentAsync(Guid customerId, Guid orderId, double amount);

    string PaymentMethodName();
}