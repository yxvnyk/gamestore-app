using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Models.DTO.Genre;

public class CreateGenreRequest
{
    [Required]
    public GenreCreateDto Genre { get; set; }
}
