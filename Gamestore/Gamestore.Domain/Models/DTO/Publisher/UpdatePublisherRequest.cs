using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Models.DTO.Publisher;

public class UpdatePublisherRequest
{
    [Required]
    public PublisherUpdateDto Publisher { get; set; }
}
