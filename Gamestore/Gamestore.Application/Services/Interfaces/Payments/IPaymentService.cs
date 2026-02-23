using Gamestore.Domain.Models.DTO.Payment;

namespace Gamestore.Application.Services.Interfaces.Payments;

public interface IPaymentService
{
    PaymentMethodsWrapper GetPaymentMethods();

    Task<PaymentResult> ProcessPaymentAsync(PaymentRequestDto paymentRequest, Guid customerId);
}
