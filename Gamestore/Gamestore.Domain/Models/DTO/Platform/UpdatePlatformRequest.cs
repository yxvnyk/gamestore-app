using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Models.DTO.Platform;

public class UpdatePlatformRequest
{
    [Required]
    public PlatformUpdateDto Platform { get; set; }
}
