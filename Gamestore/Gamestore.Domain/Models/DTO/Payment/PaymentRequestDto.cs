using Gamestore.Domain.Models.DTO.Payment.Strategy;

namespace Gamestore.Domain.Models.DTO.Payment;

public class PaymentRequestDto
{
    public string Method { get; set; }

    public VisaPayDto? Model { get; set; }
}
