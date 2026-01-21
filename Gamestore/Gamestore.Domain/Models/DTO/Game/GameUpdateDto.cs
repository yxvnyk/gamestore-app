using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Models.DTO.Game;

public class GameUpdateDto
{
    [Required]
    public Guid Id { get; set; }

    [MaxLength(50)]
    public string? Name { get; set; }

    [MaxLength(100)]
    public string? Key { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public double? Price { get; set; }

    public int? Discount { get; set; }

    public int? UnitInStock { get; set; }
}
