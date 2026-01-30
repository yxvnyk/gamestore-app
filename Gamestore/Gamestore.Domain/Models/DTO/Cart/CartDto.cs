namespace Gamestore.Domain.Models.DTO.Cart;

public class CartDto
{
    public Guid PeroductId { get; set; }

    public double Price { get; set; }

    public int Quantity { get; set; }

    public int? Discount { get; set; }
}
