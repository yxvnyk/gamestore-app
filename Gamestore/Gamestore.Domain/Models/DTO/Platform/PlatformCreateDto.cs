using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Models.DTO.Platform;

public class PlatformCreateDto
{
    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = string.Empty;
}
