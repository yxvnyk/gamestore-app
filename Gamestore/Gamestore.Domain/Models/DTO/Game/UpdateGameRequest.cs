using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Models.DTO.Game;

public class UpdateGameRequest
{
    [Required]
    public GameUpdateDto Game { get; set; }

    public Identity[]? Genres { get; set; }

    public Guid[]? Platforms { get; set; }

    public Identity? Publisher { get; set; }
}
