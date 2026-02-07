using Gamestore.Domain.Models.DTO;
using Gamestore.Domain.Models.DTO.Game;

namespace GameStore.Application.Helpers.Interfaces;

public interface IGenerateGameFile
{
    FileDto GenerateFileDto(GameDto game);
}
