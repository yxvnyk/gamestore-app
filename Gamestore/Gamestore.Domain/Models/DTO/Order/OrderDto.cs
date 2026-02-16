namespace Gamestore.Domain.Models.DTO.Order;

public class OrderDto
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public Guid CustomerId { get; set; }
}
