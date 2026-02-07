using Gamestore.Domain.Models.DTO.Comments;

namespace Gamestore.Application.Services.Interfaces;

public interface ICommentService
{
    Task CreateAsync(CommentCreateDto request, string key);
}
