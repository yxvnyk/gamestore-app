namespace Gamestore.Domain.Models.DTO.OrderItem;

public class OrderItemDto
{
    public Guid ProductId { get; set; }

    public double Price { get; set; }

    public int Quantity { get; set; }

    public int? Discount { get; set; }
}
