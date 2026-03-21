using AutoMapper;
using Gamestore.Application.Services.Integration.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models;

namespace Gamestore.Application.Services.Integration;

public class ProductIntegrationService(INorthwindProductRepository productRepository,
    IGameRepository gameRepository,
    ISupplierIntegrationService supplierIntegrationService,
    ICategoryIntegrationService categoryIntegrationService,
    IMapper mapper) : IProductIntegrationService
{
    public async Task<Guid> EnsurePromotedAsync(Identity identity)
    {
        if (identity.IsGuid)
        {
            return identity.GuidId!.Value;
        }

        var id = identity.IntId!.Value;

        var product = await productRepository.GetAsync(id) ?? throw new NotFoundException($"Game with ID {id} does not exist.");

        var promotedId = await gameRepository.GetGameIdByKeyAsync(product.GameKey);
        if (promotedId is not null && promotedId != Guid.Empty)
        {
            return promotedId.Value;
        }

        var publisherId = await supplierIntegrationService.EnsurePromotedAsync(product.SupplierId);
        var genreId = await categoryIntegrationService.EnsurePromotedAsync(product.CategoryId);

        var game = mapper.Map<Game>(product);
        game.PublisherId = publisherId;

        game.GameGenres =
        [
            new()
            {
            GenreId = genreId,
            },
        ];

        await gameRepository.CreateGameAsync(game);
        return game.Id;
    }
}
