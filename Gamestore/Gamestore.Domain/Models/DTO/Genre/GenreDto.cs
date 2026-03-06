using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Models.DTO.Genre;

public class GenreDto
{
    [Required]
    public string Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
}
