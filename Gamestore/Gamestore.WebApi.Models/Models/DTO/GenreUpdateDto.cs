using System.ComponentModel.DataAnnotations;

namespace Gamestore.WebApi.Models.Models.DTO;

public class GenreUpdateDto
{
    [Required]
    public Guid Id { get; set; }

    [MaxLength(50)]
    public string Name { get; set; }

    public Guid? ParentGenreId { get; set; }
}
