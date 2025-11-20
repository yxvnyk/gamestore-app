using Gamestore.WebApi.Models.Models.DTO;

namespace Gamestore.WebApi.Services.Interfaces;

public interface IGenreDatabaseService
{
    Task CreateGenreAsync(GenreDto genre);
}
