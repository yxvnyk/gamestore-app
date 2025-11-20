using System.ComponentModel.DataAnnotations;

namespace Gamestore.WebApi.Models.Models.DTO;

public class GenreDto
{
    [Required]
    public string Name { get; set; }

    public Guid? ParentGenreId { get; set; }
}
