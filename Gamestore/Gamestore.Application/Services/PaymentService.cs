using Gamestore.Application.Services.Interfaces;
using Gamestore.Domain.Models.Configuration;
using Gamestore.Domain.Models.DTO.PaymentMethod;
using Microsoft.Extensions.Options;

namespace Gamestore.Application.Services;

public class PaymentService(IOptions<PaymentSettings> paymentSettings) : IPaymentService
{
    private readonly PaymentSettings _paymentSettings = paymentSettings.Value;

    public PaymentMethodsResponse GetPaymentMethods()
    {
        return new PaymentMethodsResponse()
        {
            PaymentMethods = _paymentSettings.Methods,
        };
    }
}
