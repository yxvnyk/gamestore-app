using Gamestore.Domain.Models.DTO.PaymentMethod;

namespace Gamestore.Application.Services.Interfaces;

public interface IPaymentService
{
    PaymentMethodsResponse GetPaymentMethods();
}
