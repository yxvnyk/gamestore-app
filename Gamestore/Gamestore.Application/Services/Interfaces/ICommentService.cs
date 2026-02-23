using Gamestore.Domain.Models.DTO;
using Gamestore.Domain.Models.DTO.Comments;

namespace Gamestore.Application.Services.Interfaces;

public interface ICommentService
{
    Task CreateAsync(CommentCreateDto request, string key);

    Task<IEnumerable<CommentTreeDto>> GetByGameKeyAsync(string key);

    Task DeleteAsync(string key, Guid id);

    Task BanUserByName(BanDto request);
}
