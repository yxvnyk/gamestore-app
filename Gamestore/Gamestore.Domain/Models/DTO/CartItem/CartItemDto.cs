namespace Gamestore.Domain.Models.DTO.CartItem;

public class CartItemDto
{
    public Guid ProductId { get; set; }

    public double Price { get; set; }

    public int Quantity { get; set; }

    public int? Discount { get; set; }
}
