namespace Gamestore.Domain.Models.DTO.Payment;

public class BoxPaymenResult
{
    public Guid UserId { get; set; }

    public Guid OrderId { get; set; }

    public DateTime PaymentDate { get; set; }

    public double Sum { get; set; }
}
