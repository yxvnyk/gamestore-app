namespace Gamestore.Domain.Models.DTO.Comments;

public class CommentTreeDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Body { get; set; }

    public ICollection<CommentTreeDto> ChildCommnets { get; set; } = [];
}
