using AutoMapper;
using Gamestore.Application.Extensions;
using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Northwind.Entities;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.DataAccess.Wrappers;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Extensions;
using Gamestore.Domain.Generators;
using Gamestore.Domain.Helpers;
using Gamestore.Domain.Interfaces;
using Gamestore.Domain.Models;
using Gamestore.Domain.Models.DTO.Game;
using Microsoft.Extensions.Logging;

namespace Gamestore.Application.Services;

public class GameService(IGameRepository gameRepository,
    INorthwindProductRepository northwindProductRepository,
    IGameDependencyResolver dependencyResolver,
    IKeyGenerator uniqueKeyGenerator,
    IMapper mapper, ILogger<GameService> logger) : IGameService
{
    public async Task UpdateGameAsync(UpdateGameRequest updateRequest)
    {
        logger.LogTrace(nameof(this.UpdateGameAsync));

        var id = await dependencyResolver.GetGameGuidOrPromote(updateRequest.Game.Id);

        await UpdateGameAsync(updateRequest, id);
    }

    public async Task<GameDto> GetAsync(string key)
    {
        logger.LogTrace(nameof(this.GetAsync));

        var gameEntity = await gameRepository.GetGameByKeyAsync(key);
        if (gameEntity == null)
        {
            var product = await northwindProductRepository.GetAsync(key);
            return product is not null ? mapper.Map<GameDto>(product) : throw new NotFoundException($"Game with key '{key}' not found.");
        }

        return mapper.Map<GameDto>(gameEntity);
    }

    public async Task<GameDto> GetByIdAsync(Identity id)
    {
        logger.LogTrace(nameof(this.GetAsync));

        if (id.IsGuid)
        {
            var gameEntity = await gameRepository.GetByIdAsync(id.GuidId!.Value);
            if (gameEntity != null)
            {
                return mapper.Map<GameDto>(gameEntity);
            }
        }

        if (id.IsInt)
        {
            var product = await northwindProductRepository.GetAsync(id.IntId!.Value);
            if (product != null)
            {
                return mapper.Map<GameDto>(product);
            }
        }

        throw new NotFoundException($"Game with ID '{id}' not found.");
    }

    public async Task<ICollection<GameDto>> GetByPlatformAsync(Guid id)
    {
        logger.LogTrace(nameof(this.GetByPlatformAsync));

        var gameEntities = await gameRepository.GetGamesByPlatformAsync(id);
        var gameDtos = gameEntities.Select(mapper.Map<GameDto>).ToList();
        return gameDtos;
    }

    public async Task<ICollection<GameDto>> GetByGenreAsync(Identity id)
    {
        logger.LogTrace(nameof(this.GetByGenreAsync));

        if (id.IsGuid)
        {
            var game = await gameRepository.GetByGenreAsync(id.GuidId!.Value);
            if (game != null)
            {
                return mapper.Map<ICollection<GameDto>>(game);
            }
        }

        if (id.IsInt)
        {
            var product = await northwindProductRepository.GetByCategoryAsync(id.IntId!.Value);
            if (product != null)
            {
                return mapper.Map<ICollection<GameDto>>(product);
            }
        }

        return [];
    }

    public async Task<ICollection<GameDto>> GeByCompanyNameAsync(string companyName)
    {
        logger.LogTrace(nameof(this.GeByCompanyNameAsync));

        var getGamesTask = gameRepository.GetByCompanyNameAsync(companyName);
        var getProductsTask = northwindProductRepository.GetBySupplierNameAsync(companyName);

        await Task.WhenAll(getGamesTask, getProductsTask);

        var games = await getGamesTask;
        var products = await getProductsTask;

        var combinedList = RemoveProductDuplications(games, products);

        return [.. combinedList];
    }

    public async Task<GetGamesResponse> GetAllGamesAsync(GetGamesRequest request)
    {
        logger.LogTrace(nameof(this.GetAllGamesAsync));

        var getGamesTask = gameRepository.GetAllGamesAsync(request);
        var getProductsTask = northwindProductRepository.GetAllAsync(request);

        await Task.WhenAll(getGamesTask, getProductsTask);

        var games = await getGamesTask;
        var products = await getProductsTask;

        var combinedList = RemoveProductDuplications(games, products);

        var totalItemCount = combinedList.Count();
        var finalList = combinedList.ApplySorting(request.Sort).ApplyPaging(request.Page, request.PageSize);

        var result = new GetGamesResponse
        {
            Games = finalList,
            TotalPages = PaginationOptionsHelper.CalculateTotalNumberOfPages(totalItemCount, request.PageSize),
            CurrentPage = request.Page,
        };
        return result;
    }

    public async Task CreateGameAsync(CreateGameRequest createRequest)
    {
        var gameEntity = mapper.Map<Game>(createRequest);

        if (createRequest.Genres is not null)
        {
            var finalGenreIds = await dependencyResolver.ResolveAndValidateGenresAsync(createRequest.Genres);
            gameEntity.GameGenres = [.. finalGenreIds.Select(genreId => new GameGenre { GenreId = genreId })];
        }

        if (createRequest.Platforms is not null)
        {
            await dependencyResolver.ValidatePlatformsExistAsync(createRequest.Platforms);
            gameEntity.GamePlatforms = [.. createRequest.Platforms.Distinct().Select(platformId => new GamePlatform { PlatformId = platformId })];
        }

        if (createRequest.Publisher is not null)
        {
            gameEntity.PublisherId = await dependencyResolver.ResolveAndValidatePublisherAsync(createRequest.Publisher);
        }

        if (string.IsNullOrWhiteSpace(gameEntity.Key))
        {
            gameEntity.Key = await uniqueKeyGenerator.GenerateUniqueKeyAsync((IUniqueKeyRepository)gameRepository, gameEntity.Name);
        }

        await gameRepository.CreateGameAsync(gameEntity);
    }

    public async Task<bool> DeleteByKeyAsync(string key)
    {
        logger.LogTrace(nameof(this.DeleteByKeyAsync));

        return await gameRepository.DeleteByKeyAsync(key);
    }

    public async Task<int> GetTotalGamesCountAsync()
    {
        logger.LogTrace(nameof(this.GetTotalGamesCountAsync));

        return await gameRepository.GetTotalGamesCountAsync();
    }

    private async Task UpdateGameAsync(UpdateGameRequest updateRequest, Guid id)
    {
        var entity = await gameRepository.GetGameWithJoinsAsync(id)
            ?? throw new NotFoundException($"Game with ID {updateRequest.Game.Id} does not exist.");
        entity.GameGenres.Clear();
        entity.GamePlatforms.Clear();

        mapper.Map(updateRequest, entity);

        if (updateRequest.Genres is not null)
        {
            var finalGenreIds = await dependencyResolver.ResolveAndValidateGenresAsync(updateRequest.Genres);
            entity.GameGenres = [.. finalGenreIds.Select(genreId => new GameGenre { GenreId = genreId })];
        }

        if (updateRequest.Platforms is not null)
        {
            await dependencyResolver.ValidatePlatformsExistAsync(updateRequest.Platforms);
            entity.GamePlatforms = [.. updateRequest.Platforms.Distinct().Select(platformId => new GamePlatform { PlatformId = platformId })];
        }

        if (updateRequest.Publisher is not null)
        {
            entity.PublisherId = await dependencyResolver.ResolveAndValidatePublisherAsync(updateRequest.Publisher);
        }

        await SynchrnizeWithProduct(entity);
        await gameRepository.UpdateGameAsync(entity);
    }

    private async Task SynchrnizeWithProduct(Game entity)
    {
        try
        {
            await northwindProductRepository.SetUnitsInStockAsync(entity.Key, entity.UnitsInStock);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "CRITICAL: Failed to sync UnitsInStock for Game {Key} in MongoDB. SQL is updated, but Mongo is out of sync. Manual intervention required.", entity.Key);
        }
    }

    private IEnumerable<GameDto> RemoveProductDuplications(IEnumerable<GameWithStats> games, IEnumerable<Product> products)
    {
        var gameKeys = games.Select(g => g.Game.Key.ToLowerInvariant()).ToHashSet();
        var uniqueProducts = products.Where(p => !gameKeys.Contains(p.GameKey.ToLowerInvariant()))
            .ToList();

        var gameDtos = mapper.Map<IEnumerable<GameDto>>(games);
        var combinedList = gameDtos.Concat(mapper.Map<IEnumerable<GameDto>>(uniqueProducts));
        return combinedList;
    }

    private IEnumerable<GameDto> RemoveProductDuplications(IEnumerable<Game> games, IEnumerable<Product> products)
    {
        var gameKeys = games.Select(g => g.Key.ToLowerInvariant()).ToHashSet();
        var uniqueProducts = products.Where(p => !gameKeys.Contains(p.GameKey.ToLowerInvariant()))
            .ToList();

        var gameDtos = mapper.Map<IEnumerable<GameDto>>(games);
        var combinedList = gameDtos.Concat(mapper.Map<IEnumerable<GameDto>>(uniqueProducts));
        return combinedList;
    }
}