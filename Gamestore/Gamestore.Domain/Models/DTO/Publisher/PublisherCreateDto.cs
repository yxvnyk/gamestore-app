using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Models.DTO.Publisher;

public class PublisherCreateDto
{
    [Required]
    [MaxLength(100)]
    public string CompanyName { get; set; }

    [Required]
    [Url]
    public string HomePage { get; set; }

    [Required]
    [MaxLength(500)]
    public string Description { get; set; }
}
