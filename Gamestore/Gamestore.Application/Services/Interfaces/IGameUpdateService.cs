using Gamestore.Domain.Models.DTO.Game;

namespace Gamestore.Application.Services.Interfaces;

public interface IGameUpdateService
{
    Task UpdateGameAsync(UpdateGameRequest updateRequest, Guid id);
}
