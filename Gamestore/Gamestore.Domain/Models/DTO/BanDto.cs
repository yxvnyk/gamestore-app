using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Models.DTO;

public class BanDto
{
    [Required]
    public string User { get; set; }

    [Required]
    public string Duration { get; set; }
}
