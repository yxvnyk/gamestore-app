using System.ComponentModel.DataAnnotations;

namespace Gamestore.WebApi.Models.Models.DTO;

public class PlatformUpdateDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = string.Empty;
}
