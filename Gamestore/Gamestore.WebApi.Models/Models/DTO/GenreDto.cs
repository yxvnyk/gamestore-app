using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Models.DTO;

public class GenreDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
}
