using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Models.DTO.Platform;

public class CreatePlatformRequest
{
    [Required]
    public PlatformCreateDto Platform { get; set; }
}
