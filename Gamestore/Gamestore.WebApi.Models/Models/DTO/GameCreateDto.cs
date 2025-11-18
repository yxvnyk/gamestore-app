using System.ComponentModel.DataAnnotations;

namespace Gamestore.WebApi.Models.Models.DTO;

public class GameCreateDto
{
    [Required]
    public GameDto Game { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one genre is required.")]
    public Guid[] Genres { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one platform is required.")]
    public Guid[] Platforms { get; set; }
}
