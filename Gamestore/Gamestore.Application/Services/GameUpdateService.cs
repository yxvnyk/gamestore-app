using AutoMapper;
using Gamestore.Application.Services.Integration.Interfaces;
using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models;
using Gamestore.Domain.Models.DTO.Game;

namespace Gamestore.Application.Services;

public class GameUpdateService(IGameRepository gameRepository,
    IGenreRepository genreRepository,
    IPlatformRepository platformRepository,
    ICategoryIntegrationService categoryIntegrationService,
    ISupplierIntegrationService supplierIntegrationService,
    IPublisherRepository publisherRepository,
    IMapper mapper) : IGameUpdateService
{
    public async Task UpdateGameAsync(UpdateGameRequest updateRequest, Guid id)
    {
        var entity = await gameRepository.GetGameWithJoinsAsync(id)
            ?? throw new NotFoundException($"Game with ID {updateRequest.Game.Id} does not exist.");
        entity.GameGenres.Clear();
        entity.GamePlatforms.Clear();

        mapper.Map(updateRequest, entity);

        if (updateRequest.Genres is not null)
        {
            var finalGenreIds = await ResolveAndValidateGenresAsync(updateRequest.Genres);
            entity.GameGenres = [.. finalGenreIds.Select(genreId => new GameGenre { GenreId = genreId })];
        }

        if (updateRequest.Platforms is not null)
        {
            await ValidatePlatformsExistAsync(updateRequest.Platforms, platformRepository.PlatformExistsAsync, "Platform");
            entity.GamePlatforms = [.. updateRequest.Platforms.Distinct().Select(platformId => new GamePlatform { PlatformId = platformId })];
        }

        if (updateRequest.Publisher is not null)
        {
            entity.PublisherId = await ResolveAndValidatePublisherAsync(updateRequest.Publisher);
        }

        await gameRepository.UpdateGameAsync(entity);
    }

    private async Task<IEnumerable<Guid>> ResolveAndValidateGenresAsync(IEnumerable<Identity> ids)
    {
        var uniqueIds = ids.Distinct().ToList();
        var finalGuids = new List<Guid>();

        foreach (var identity in uniqueIds)
        {
            var guid = await GetGenreGuidOrPromote(identity);

            if (!await genreRepository.GenreExistsAsync(guid))
            {
                throw new NotFoundException($"Genre with ID {guid} does not exist.");
            }

            finalGuids.Add(guid);
        }

        return finalGuids;
    }

    private async Task<Guid> ResolveAndValidatePublisherAsync(Identity id)
    {
        var guid = await GetPublisherGuidOrPromote(id);

        return !await publisherRepository.PublisherExistAsync(guid)
            ? throw new NotFoundException($"Publisher with ID {guid} does not exist.")
            : guid;
    }

    private static async Task ValidatePlatformsExistAsync(IEnumerable<Guid> ids, Func<Guid, Task<bool>> isExistFunc, string entityName)
    {
        var uniqueIds = ids.Distinct();
        foreach (var id in uniqueIds)
        {
            if (!await isExistFunc(id))
            {
                throw new NotFoundException($"{entityName} with ID {id} does not exist.");
            }
        }
    }

    private async Task<Guid> GetGenreGuidOrPromote(Identity id)
    {
        if (id.IsGuid)
        {
            return id.GuidId!.Value;
        }

        // return
        return await categoryIntegrationService.EnsurePromotedAsync(id.IntId!.Value);
    }

    private async Task<Guid> GetPublisherGuidOrPromote(Identity id)
    {
        if (id.IsGuid)
        {
            return id.GuidId!.Value;
        }

        // return
        return await supplierIntegrationService.EnsurePromotedAsync(id.IntId!.Value);
    }
}