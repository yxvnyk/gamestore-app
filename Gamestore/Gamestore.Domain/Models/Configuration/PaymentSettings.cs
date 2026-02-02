using Gamestore.Domain.Models.DTO.Payment;

namespace Gamestore.Domain.Models.Configuration;

public class PaymentSettings
{
    public List<PaymentMethodDto> Methods { get; set; }

    public int BankPaymentValidityDate { get; set; }
}
