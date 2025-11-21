using Gamestore.WebApi.Models.Models.DTO;

namespace Gamestore.WebApi.Services.Interfaces;

public interface IGenerateGameFile
{
    FileDto GenerateFileDto(GameDto game);
}
