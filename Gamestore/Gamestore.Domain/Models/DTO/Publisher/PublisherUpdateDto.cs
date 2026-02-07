using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Models.DTO.Publisher;

public class PublisherUpdateDto
{
    [Required]
    public Guid Id { get; set; }

    [MaxLength(100)]
    public string? CompanyName { get; set; }

    [Url]
    public string? HomePage { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
}
