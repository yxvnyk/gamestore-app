using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.WebApi.Models.Models.DTO;
using Gamestore.WebApi.Services.Interfaces;

namespace Gamestore.WebApi.Services;

public class GenreDatabaseService(IGenreRepository genreRepository, IMapper mapper) : IGenreDatabaseService
{
    public async Task CreateGenreAsync(GenreCreateDto genre)
    {
        if (genre.ParentGenreId != null)
        {
            if (!await genreRepository.GenreExistsAsync(genre.ParentGenreId.Value))
            {
                throw new ArgumentException($"Parent genre with ID {genre.ParentGenreId} does not exist.");
            }
        }

        var enity = mapper.Map<GenreCreateDto, GenreEntity>(genre);
        await genreRepository.CreateGenreAsync(enity);
    }

    public async Task<IEnumerable<GenreDto>> GetAllGenresAsync()
    {
        var genreEntities = await genreRepository.GetAllGenresAsync();
        return [.. genreEntities.Select(mapper.Map<GenreDto>)];
    }

    public async Task<GenreDto> GetGenreByGameKeyAsync(string key)
    {
        var genreEntity = await genreRepository.GetGenreByGameKeyAsync(key);
        return genreEntity is not null ? mapper.Map<GenreDto>(genreEntity) : null;
    }

    public async Task<GenreFullDto?> GetGenreByIdAsync(Guid id)
    {
        var genreEntity = await genreRepository.GetGenreByIdAsync(id);
        return genreEntity is not null ? mapper.Map<GenreFullDto>(genreEntity) : null;
    }

    public async Task<ICollection<GenreDto>> GetGenresByParentIdAsync(Guid id)
    {
        if (!await genreRepository.GenreExistsAsync(id))
        {
            throw new ArgumentException($"Parent genre with ID {id} does not exist.");
        }

        var genreEntities = await genreRepository.GetGenresByParentIdAsync(id);
        return [.. genreEntities.Select(mapper.Map<GenreDto>)];
    }

    public async Task UpdateGenreAsync(GenreUpdateDto model)
    {
        var entity = await genreRepository.GetGenreByIdAsync(model.Id) ?? throw new ArgumentException($"Game with ID {model.Id} does not exist.");
        mapper.Map(model, entity);
        await genreRepository.SaveChangesAsync();
    }
}
