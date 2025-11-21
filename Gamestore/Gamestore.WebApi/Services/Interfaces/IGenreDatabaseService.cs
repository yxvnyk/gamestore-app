using Gamestore.WebApi.Models.Models.DTO;

namespace Gamestore.WebApi.Services.Interfaces;

public interface IGenreDatabaseService
{
    Task CreateGenreAsync(GenreCreateDto genre);

    Task<GenreFullDto?> GetGenreByIdAsync(Guid id);

    Task<IEnumerable<GenreDto>> GetAllGenresAsync();

    Task<GenreDto> GetGenreByGameKeyAsync(string key);

    Task<ICollection<GenreDto>> GetGenresByParentIdAsync(Guid id);

    Task UpdateGenreAsync(GenreUpdateDto model);

    Task<bool> DeleteByIdAsync(Guid id);
}
