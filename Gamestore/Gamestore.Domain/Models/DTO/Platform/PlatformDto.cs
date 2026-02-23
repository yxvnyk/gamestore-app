using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Models.DTO.Platform;

public class PlatformDto
{
    [Required]
    public Guid Id { get; set; }

    [MaxLength(50)]
    public string Type { get; set; } = string.Empty;
}
