namespace Gamestore.Domain.Models.DTO.Payment.Strategy;

public class PaymentContextDto
{
    public Guid CustomerId { get; set; }

    public Guid OrderId { get; set; }

    public double Amount { get; set; }

    public VisaPayDto? VisaModel { get; set; }
}
