using System.Text.Json.Serialization;
using Gamestore.Domain.Convertors;

namespace Gamestore.Domain.Models.DTO.Comments;

public class CommentCreateDto
{
    public CommentDto Comment { get; set; }

    [JsonConverter(typeof(EmptyStringNullableGuidConverter))]
    public Guid? ParentId { get; set; }

    public string? Action { get; set; }
}
