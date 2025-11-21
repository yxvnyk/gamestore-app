using System.ComponentModel.DataAnnotations;

namespace Gamestore.WebApi.Models.Models.DTO;

public class PlatformDto
{
    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = string.Empty;
}
