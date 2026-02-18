using System.ComponentModel.DataAnnotations;

namespace Gamestore.Domain.Models.DTO.Comments;

public class CommentDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    [MaxLength(500)]
    public string Body { get; set; }
}
