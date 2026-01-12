using Gamestore.Domain.Models.DTO;

namespace Gamestore.Application.Services.Interfaces;

public interface IGenreService
{
    Task CreateGenreAsync(GenreCreateDto genre);

    Task<GenreFullDto?> GetGenreByIdAsync(Guid id);

    Task<IEnumerable<GenreDto>> GetAllGenresAsync();

    Task<IEnumerable<GenreDto>> GetGenresByGameKeyAsync(string key);

    Task<List<GenreDto>> GetGenresByParentIdAsync(Guid id);

    Task UpdateGenreAsync(GenreUpdateDto model);

    Task<bool> DeleteByIdAsync(Guid id);
}
