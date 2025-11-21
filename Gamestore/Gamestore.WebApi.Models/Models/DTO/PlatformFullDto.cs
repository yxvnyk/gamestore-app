using System.ComponentModel.DataAnnotations;

namespace Gamestore.WebApi.Models.Models.DTO;

public class PlatformFullDto
{
    public Guid Id { get; set; }

    [MaxLength(50)]
    public string Type { get; set; } = string.Empty;
}
