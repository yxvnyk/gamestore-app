using Gamestore.Domain.Models.DTO.Payment;
using Gamestore.Domain.Models.DTO.Payment.Strategy;

namespace Gamestore.Application.Helpers.Interfaces;

public interface IPaymentStrategy
{
    Task<PaymentResult> ProcessPaymentAsync(SimplePayDto payDto);

    string PaymentMethodName();
}