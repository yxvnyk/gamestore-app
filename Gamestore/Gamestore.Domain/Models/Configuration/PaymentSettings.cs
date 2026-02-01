using Gamestore.Domain.Models.DTO.PaymentMethod;

namespace Gamestore.Domain.Models.Configuration;

public class PaymentSettings
{
    public List<PaymentMethodDto> Methods { get; set; }
}
