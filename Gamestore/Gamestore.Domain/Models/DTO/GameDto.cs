using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Models.DTO;

public class GameDto
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string? Name { get; set; }

    [MaxLength(100)]
    public string? Key { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public double Price { get; set; }

    [Required]
    public int Discount { get; set; }

    [Required]
    public int UnitInStock { get; set; }
}
