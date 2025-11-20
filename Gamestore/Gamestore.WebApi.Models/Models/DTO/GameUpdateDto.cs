using System.ComponentModel.DataAnnotations;

namespace Gamestore.WebApi.Models.Models.DTO;

public class GameUpdateDto
{
    [Required]
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Key { get; set; }

    public string? Description { get; set; }
}
