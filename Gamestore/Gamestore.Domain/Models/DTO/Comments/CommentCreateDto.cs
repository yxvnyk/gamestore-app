namespace Gamestore.Domain.Models.DTO.Comments;

public class CommentCreateDto
{
    public CommentDto Comment { get; set; }

    public Guid? ParentId { get; set; }

    public string? Action { get; set; }
}
