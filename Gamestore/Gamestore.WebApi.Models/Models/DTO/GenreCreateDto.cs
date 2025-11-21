using System.ComponentModel.DataAnnotations;

namespace Gamestore.WebApi.Models.Models.DTO;

public class GenreCreateDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    public Guid? ParentGenreId { get; set; }
}
