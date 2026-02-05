namespace Gamestore.Domain.Models.DTO.Payment.Strategy;

public class SimplePayDto
{
    public Guid CustomerId { get; set; }

    public Guid OrderId { get; set; }

    public double Amount { get; set; }

    public VisaPayDto? VisaDetails { get; set; }
}
