using Gamestore.Application.Services.Integration.Interfaces;
using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models;

namespace Gamestore.Application.Services;

public class GameDependencyResolver(
    IGenreRepository genreRepository,
    IPlatformRepository platformRepository,
    IProductIntegrationService productIntegrationService,
    ICategoryIntegrationService categoryIntegrationService,
    ISupplierIntegrationService supplierIntegrationService,
    IPublisherRepository publisherRepository) : IGameDependencyResolver
{
    public async Task<IEnumerable<Guid>> ResolveAndValidateGenresAsync(IEnumerable<Identity> ids)
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

    public async Task<Guid> ResolveAndValidatePublisherAsync(Identity id)
    {
        var guid = await GetPublisherGuidOrPromote(id);

        return !await publisherRepository.PublisherExistAsync(guid)
            ? throw new NotFoundException($"Publisher with ID {guid} does not exist.")
            : guid;
    }

    public async Task ValidatePlatformsExistAsync(IEnumerable<Guid> ids)
    {
        var uniqueIds = ids.Distinct();
        foreach (var id in uniqueIds)
        {
            if (!await platformRepository.PlatformExistsAsync(id))
            {
                throw new NotFoundException($"Platform with ID {id} does not exist.");
            }
        }
    }

    public async Task<Guid> GetGameGuidOrPromote(Identity id)
    {
        if (id.IsGuid)
        {
            return id.GuidId!.Value;
        }

        // return
        return await productIntegrationService.EnsurePromotedAsync(id.IntId!.Value);
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