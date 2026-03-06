using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Gamestore.Domain.Models.DTO.Game;

public class GameDto
{
    public string? Id { get; set; }

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

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? CommentCount { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime? CreatedDate { get; set; }
}
