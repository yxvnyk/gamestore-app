using AutoMapper;
using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Extensions;
using Gamestore.Domain.Models.DTO;
using Microsoft.Extensions.Logging;

namespace Gamestore.Application.Services;

public class GenreService(IGenreRepository genreRepository,
    IGameRepository gameRepository, IMapper mapper, ILogger<GenreService> logger) : IGenreService
{
    public async Task CreateGenreAsync(GenreCreateDto genre)
    {
        logger.LogTrace(nameof(this.CreateGenreAsync));

        if (genre.ParentGenreId != null && !await genreRepository.GenreExistsAsync(genre.ParentGenreId.Value))
        {
            throw new ArgumentException($"Parent genre with ID {genre.ParentGenreId} does not exist.");
        }

        var enity = mapper.Map<GenreCreateDto, Genre>(genre);
        await genreRepository.CreateGenreAsync(enity);
    }

    public async Task<IEnumerable<GenreDto>> GetAllGenresAsync()
    {
        logger.LogTrace(nameof(this.GetAllGenresAsync));

        var genreEntities = await genreRepository.GetAllGenresAsync();
        return [.. genreEntities.Select(mapper.Map<GenreDto>)];
    }

    public async Task<IEnumerable<GenreDto>> GetGenresByGameKeyAsync(string key)
    {
        logger.LogTrace(nameof(this.GetGenresByGameKeyAsync));

        var gameExists = await gameRepository.GameKeyExistAsync(key);
        if (!gameExists)
        {
            throw new NotFoundException($"Game with key '{key}' not found.");
        }

        var genreEntities = await genreRepository.GetGenresByGameKeyAsync(key);
        return [.. genreEntities.Select(mapper.Map<GenreDto>)];
    }

    public async Task<GenreFullDto?> GetGenreByIdAsync(Guid id)
    {
        logger.LogTrace(nameof(this.GetGenreByIdAsync));

        var genreEntity = await genreRepository.GetGenreByIdAsync(id);
        return genreEntity is not null ? mapper.Map<GenreFullDto>(genreEntity) : throw new NotFoundException($"Genre with ID '{id}' not found.");
    }

    public async Task<List<GenreDto>> GetGenresByParentIdAsync(Guid id)
    {
        logger.LogTrace(nameof(this.GetGenresByParentIdAsync));

        if (!await genreRepository.GenreExistsAsync(id))
        {
            throw new NotFoundException($"Parent genre with ID {id} does not exist.");
        }

        var genreEntities = await genreRepository.GetGenresByParentIdAsync(id);
        return [.. genreEntities.Select(mapper.Map<GenreDto>)];
    }

    public async Task UpdateGenreAsync(GenreUpdateDto model)
    {
        logger.LogTrace(nameof(this.UpdateGenreAsync));

        var entity = await genreRepository.GetGenreByIdAsync(model.Id) ?? throw new NotFoundException($"Genre with ID {model.Id} does not exist.");
        mapper.Map(model, entity);
        await genreRepository.UpdateGenreAsync(entity);
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        logger.LogTrace(nameof(this.DeleteByIdAsync));

        return await genreRepository.DeleteByIdAsync(id);
    }
}
