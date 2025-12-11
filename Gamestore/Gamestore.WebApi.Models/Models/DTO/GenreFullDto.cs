using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Models.DTO;

public class GenreFullDto
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    public Guid? ParentGenreId { get; set; }
}
