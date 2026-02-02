using Gamestore.Domain.Models.DTO.Payment;

namespace Gamestore.Application.Services.Interfaces;

public interface IPaymentService
{
    PaymentMethodsResponse GetPaymentMethods();

    Task<PaymentResult> ProcessPaymentAsync(PaymentRequestDto paymentRequest, Guid customerId);
}
