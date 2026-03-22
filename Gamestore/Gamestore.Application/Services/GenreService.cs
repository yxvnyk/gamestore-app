using AutoMapper;
using Gamestore.Application.Services.Integration.Interfaces;
using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Northwind.Entities;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Extensions;
using Gamestore.Domain.Models;
using Gamestore.Domain.Models.DTO.Genre;
using Microsoft.Extensions.Logging;

namespace Gamestore.Application.Services;

public class GenreService(IGenreRepository genreRepository,
    IGameRepository gameRepository,
    INorthwindCategoryRepository northwindCategoryRepository,
    ICategoryIntegrationService categoryIntegrationService,
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

        var genreDtos = RemoveGenreDuplications(genres, categories);

        return genreDtos;
    }

    public async Task<IEnumerable<GenreDto>> GetGenresByGameKeyAsync(string key)
    {
        logger.LogTrace(nameof(this.GetGenresByGameKeyAsync));

        var sqlGameExistsTask = gameRepository.GameKeyExistAsync(key);
        var mongoCategoryTask = northwindCategoryRepository.GetByGameKeyAsync(key);

        await Task.WhenAll(sqlGameExistsTask, mongoCategoryTask);

        var isSqlGameFound = sqlGameExistsTask.Result;
        var mongoCategory = mongoCategoryTask.Result;

        if (!isSqlGameFound && mongoCategory == null)
        {
            throw new NotFoundException($"Game with key '{key}' not found.");
        }

        var genres = new List<GenreDto>();

        if (isSqlGameFound)
        {
            var sqlGenres = await genreRepository.GetGenresByGameKeyAsync(key);
            if (sqlGenres != null && sqlGenres.Any())
            {
                genres.AddRange(mapper.Map<IEnumerable<GenreDto>>(sqlGenres));
            }
        }

        if (mongoCategory != null)
        {
            genres.Add(mapper.Map<GenreDto>(mongoCategory));
        }

        return [.. genres.DistinctBy(g => g.Name.ToLowerInvariant())];
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

        var categoryEntity = await northwindCategoryRepository.GetAsync(id.IntId!.Value);
        return categoryEntity != null
            ? mapper.Map<GenreFullDto>(categoryEntity)
            : throw new NotFoundException($"Genre with ID '{id}' not found.");
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

        var id = model.Id;

        if (id.IsGuid)
        {
            var entity = await genreRepository.GetGenreByIdAsync(id.GuidId!.Value) ?? throw new NotFoundException($"Genre with ID {id.GuidId!.Value} does not exist.");
            mapper.Map(model, entity);
            await genreRepository.UpdateGenreAsync(entity);

            return;
        }

        await categoryIntegrationService.PromoteToGenreAndUpdateAsync(model);
    }

    public async Task<bool> DeleteByIdAsync(Identity identity)
    {
        logger.LogTrace(nameof(this.DeleteByIdAsync));

        return identity.IsInt
            ? throw new BusinessRuleValidationException("Legacy items from Northwind database cannot be deleted.")
            : await genreRepository.DeleteByIdAsync(identity.GuidId!.Value);
    }

    private IEnumerable<GenreDto> RemoveGenreDuplications(IEnumerable<Genre> genres, IEnumerable<Category> categories)
    {
        var gameLegacyIds = genres.Select(g => g.LegacyId).ToHashSet();
        var uniqueProducts = categories.Where(p => !gameLegacyIds.Contains(p.CategoryId))
            .ToList();

        var genreDtos = mapper.Map<IEnumerable<GenreDto>>(genres);
        var combinedList = genreDtos.Concat(mapper.Map<IEnumerable<GenreDto>>(uniqueProducts));
        return combinedList;
    }
}
