using Gamestore.Domain.Models;
using Gamestore.Domain.Models.DTO.Genre;

namespace Gamestore.Application.Services.Interfaces;

public interface IGenreService
{
    Task CreateGenreAsync(GenreCreateDto genre);

    Task<GenreFullDto?> GetGenreByIdAsync(Identity id);

    Task<IEnumerable<GenreDto>> GetAllGenresAsync();

    Task<IEnumerable<GenreDto>> GetGenresByGameKeyAsync(string key);

    Task<List<GenreDto>> GetGenresByParentIdAsync(Identity id);

    Task UpdateGenreAsync(GenreUpdateDto model);

    Task<bool> DeleteByIdAsync(Identity identity);
}
