using Gamestore.Domain.Models.DTO;

namespace GameStore.Application.Helpers.Interfaces;

public interface IGenerateGameFile
{
    FileDto GenerateFileDto(GameDto game);
}
