using Gamestore.Domain.Models.DTO.Payment;
using Gamestore.Domain.Models.DTO.Payment.Strategy;

namespace Gamestore.Application.Services.Interfaces.Payments;

public interface IPaymentStrategy
{
    Task<PaymentResult> ProcessPaymentAsync(PaymentContextDto context);

    string PaymentMethodName();
}