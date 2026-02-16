using Gamestore.Domain.Models.Configuration;

namespace Gamestore.Domain.Models.DTO.Payment;

public class PaymentMethodsWrapper
{
    public List<PaymentMethodDto> PaymentMethods { get; set; }
}
