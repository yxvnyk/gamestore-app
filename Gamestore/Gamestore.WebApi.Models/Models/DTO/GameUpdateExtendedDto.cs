using System.ComponentModel.DataAnnotations;

namespace Gamestore.WebApi.Models.Models.DTO;

public class GameUpdateExtendedDto
{
    [Required]
    public GameUpdateDto Game { get; set; }

    public Guid[]? Genres { get; set; }

    public Guid[]? Platforms { get; set; }
}
