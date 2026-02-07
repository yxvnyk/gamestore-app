using AutoMapper;
using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models.DTO.Comments;
using Microsoft.Extensions.Logging;

namespace Gamestore.Application.Services;

public class CommentService(ICommentRepository commentRepository, IGameRepository gameRepository,
    IMapper mapper, ILogger<PublisherService> logger) : ICommentService
{
    public async Task CreateAsync(CommentCreateDto request, string key)
    {
        logger.LogTrace(nameof(this.CreateAsync));

        var gameId = await gameRepository.GetIdByKeyAsync(key) ?? throw new NotFoundException($"Game with key {key} not found");

        if (request.ParentId is not null)
        {
            if (!await commentRepository.IsExistAsync(request.ParentId.Value))
            {
                throw new NotFoundException($"Comment with id {request.ParentId} not found");
            }
        }

        var comment = mapper.Map<Comment>(request);
        comment.GameId = gameId;

        await commentRepository.CreateAsync(comment);
    }
}
