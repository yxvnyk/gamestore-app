using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Models.DTO.Genre;

public class GenreCreateDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    public Guid? ParentGenreId { get; set; }
}
