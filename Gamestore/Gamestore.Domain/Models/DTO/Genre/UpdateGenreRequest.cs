using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Models.DTO.Genre;

public class UpdateGenreRequest
{
    [Required]
    public GenreUpdateDto Genre { get; set; }
}
