using AutoMapper;
using Gamestore.Application.Models;
using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Extensions;
using Gamestore.Domain.Models.DTO.Genre;
using Microsoft.Extensions.Logging;

namespace Gamestore.Application.Services;

public class GenreService(IGenreRepository genreRepository,
    IGameRepository gameRepository,
    INorthwindCategoryRepository northwindCategoryRepository,
    IMapper mapper, ILogger<GenreService> logger) : IGenreService
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

        var genreTask = genreRepository.GetAllGenresAsync();
        var categoryTask = northwindCategoryRepository.GetAllAsync();

        await Task.WhenAll(genreTask, categoryTask);

        var genres = await genreTask;
        var categories = await categoryTask;

        var genreDtos = mapper.Map<IEnumerable<GenreDto>>(genres);
        genreDtos = genreDtos.Concat(mapper.Map<IEnumerable<GenreDto>>(categories));

        return genreDtos;
    }

    public async Task<IEnumerable<GenreDto>> GetGenresByGameKeyAsync(string key)
    {
        logger.LogTrace(nameof(this.GetGenresByGameKeyAsync));

        var gameExists = await gameRepository.GameKeyExistAsync(key);
        if (gameExists)
        {
            var genreEntities = await genreRepository.GetGenresByGameKeyAsync(key);
            if (genreEntities != null)
            {
                return mapper.Map<IEnumerable<GenreDto>>(genreEntities);
            }
        }

        var categoryExists = await northwindCategoryRepository.GameKeyExistAsync(key);
        if (categoryExists)
        {
            var categoryEntity = await northwindCategoryRepository.GetByGameKeyAsync(key);
            if (categoryEntity == null)
            {
                return [];
            }

            List<GenreDto> categories = [];
            categories.Add(mapper.Map<GenreDto>(categoryEntity));
            return categories;
        }

        throw new NotFoundException($"Game with key '{key}' not found.");
    }

    public async Task<GenreFullDto?> GetGenreByIdAsync(Identity id)
    {
        logger.LogTrace(nameof(this.GetGenreByIdAsync));

        if (id.IsGuid)
        {
            var genreEntity = await genreRepository.GetGenreByIdAsync(id.GuidId!.Value);
            if (genreEntity != null)
            {
                return mapper.Map<GenreFullDto>(genreEntity);
            }
        }

        if (id.IsInt)
        {
            var genreEntity = await northwindCategoryRepository.GetAsync(id.IntId!.Value);
            if (genreEntity != null)
            {
                return mapper.Map<GenreFullDto>(genreEntity);
            }
        }

        throw new NotFoundException($"Genre with ID '{id}' not found.");
    }

    public async Task<List<GenreDto>> GetGenresByParentIdAsync(Identity id)
    {
        logger.LogTrace(nameof(this.GetGenresByParentIdAsync));

        if (!id.IsGuid)
        {
            return [];
        }

        if (!await genreRepository.GenreExistsAsync(id.GuidId!.Value))
        {
            throw new NotFoundException($"Parent genre with ID {id} does not exist.");
        }

        var genreEntities = await genreRepository.GetGenresByParentIdAsync(id.GuidId!.Value);
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
