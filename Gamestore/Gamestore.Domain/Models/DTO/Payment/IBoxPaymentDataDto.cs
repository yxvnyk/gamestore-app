namespace Gamestore.Domain.Models.DTO.Payment;

public class IBoxPaymentDataDto
{
    public Guid UserId { get; set; }

    public Guid OrderId { get; set; }

    public DateTime PaymentData { get; set; }

    public double Sum { get; set; }
}
