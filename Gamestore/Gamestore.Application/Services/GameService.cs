using AutoMapper;
using Gamestore.Application.Extensions;
using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Extensions;
using Gamestore.Domain.Generators;
using Gamestore.Domain.Helpers;
using Gamestore.Domain.Interfaces;
using Gamestore.Domain.Models;
using Gamestore.Domain.Models.DTO.Game;
using Microsoft.Extensions.Logging;

namespace Gamestore.Application.Services;

public class GameService(IGameRepository gameRepository, INorthwindProductRepository northwindProductRepository,
    IGenreRepository genreRepository, IPlatformRepository platformRepository, IPublisherRepository publisherRepository,
    IKeyGenerator uniqueKeyGenerator,
    IMapper mapper, ILogger<GameService> logger) : IGameService
{
    private readonly IGameRepository _gameRepository = gameRepository;
    private readonly INorthwindProductRepository _northwindProductRepository = northwindProductRepository;
    private readonly IGenreRepository _genreRepository = genreRepository;
    private readonly IPlatformRepository _platformRepository = platformRepository;
    private readonly IPublisherRepository _publisherRepository = publisherRepository;
    private readonly IKeyGenerator _uniqueKeyGenerator = uniqueKeyGenerator;
    private readonly IMapper _mapper = mapper;

    public Task UpdateGameAsync(UpdateGameRequest updateRequest)
    {
        logger.LogTrace(nameof(this.UpdateGameAsync));

        throw new NotImplementedException();
    }

    public async Task<GameDto> GetAsync(string key)
    {
        logger.LogTrace(nameof(this.GetAsync));

        var gameEntity = await _gameRepository.GetGameByKeyAsync(key);
        if (gameEntity == null)
        {
            var product = await _northwindProductRepository.GetAsync(key);
            return product is not null ? _mapper.Map<GameDto>(product) : throw new NotFoundException($"Game with key '{key}' not found.");
        }

        return _mapper.Map<GameDto>(gameEntity);
    }

    public async Task<GameDto> GetByIdAsync(Identity id)
    {
        logger.LogTrace(nameof(this.GetAsync));

        if (id.IsGuid)
        {
            var gameEntity = await _gameRepository.GetByIdAsync(id.GuidId!.Value);
            if (gameEntity != null)
            {
                return _mapper.Map<GameDto>(gameEntity);
            }
        }

        if (id.IsInt)
        {
            var product = await _northwindProductRepository.GetAsync(id.IntId!.Value);
            if (product != null)
            {
                return _mapper.Map<GameDto>(product);
            }
        }

        throw new NotFoundException($"Game with ID '{id}' not found.");
    }

    public async Task<ICollection<GameDto>> GetByPlatformAsync(Guid id)
    {
        logger.LogTrace(nameof(this.GetByPlatformAsync));

        var gameEntities = await _gameRepository.GetGamesByPlatformAsync(id);
        var gameDtos = gameEntities.Select(_mapper.Map<GameDto>).ToList();
        return gameDtos;
    }

    public async Task<ICollection<GameDto>> GetByGenreAsync(Identity id)
    {
        logger.LogTrace(nameof(this.GetByGenreAsync));

        if (id.IsGuid)
        {
            var game = await _gameRepository.GetByGenreAsync(id.GuidId!.Value);
            if (game != null)
            {
                return _mapper.Map<ICollection<GameDto>>(game);
            }
        }

        if (id.IsInt)
        {
            var product = await _northwindProductRepository.GetByCategoryAsync(id.IntId!.Value);
            if (product != null)
            {
                return _mapper.Map<ICollection<GameDto>>(product);
            }
        }

        return [];
    }

    public async Task<ICollection<GameDto>> GeByCompanyNameAsync(string companyName)
    {
        logger.LogTrace(nameof(this.GeByCompanyNameAsync));

        var getGamesTask = _gameRepository.GetByCompanyNameAsync(companyName);
        var getProductsTask = _northwindProductRepository.GetBySupplierNameAsync(companyName);

        await Task.WhenAll(getGamesTask, getProductsTask);

        var games = await getProductsTask;
        var products = await getProductsTask;

        var gameDtos = _mapper.Map<IEnumerable<GameDto>>(games);
        var combinedList = gameDtos.Concat(_mapper.Map<IEnumerable<GameDto>>(products));

        return [.. combinedList];
    }

    public async Task<GetGamesResponse> GetAllGamesAsync(GetGamesRequest request)
    {
        logger.LogTrace(nameof(this.GetAllGamesAsync));

        var getGamesTask = _gameRepository.GetAllGamesAsync(request);
        var getProductsTask = _northwindProductRepository.GetAllAsync(request);

        await Task.WhenAll(getGamesTask, getProductsTask);

        var games = await getGamesTask;
        var products = await getProductsTask;

        var gameDtos = _mapper.Map<IEnumerable<GameDto>>(games);
        var combinedList = gameDtos.Concat(_mapper.Map<IEnumerable<GameDto>>(products));

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
        logger.LogTrace(nameof(this.CreateGameAsync));

        await ValidateGamestoreEntitiesExistAsync(createRequest.Genres, _genreRepository.GenreExistsAsync, "Genre");
        await ValidateGamestoreEntitiesExistAsync(createRequest.Platforms, _platformRepository.PlatformExistsAsync, "Platform");
        await ValidatePublisherExistAsync(createRequest.Publisher);

        createRequest.Genres = [.. createRequest.Genres.Distinct()];
        createRequest.Platforms = [.. createRequest.Platforms.Distinct()];
        var gameEntity = _mapper.Map<Game>(createRequest);

        if (string.IsNullOrWhiteSpace(gameEntity.Key))
        {
            gameEntity.Key = await _uniqueKeyGenerator.GenerateUniqueKeyAsync((IUniqueKeyRepository)_gameRepository, gameEntity.Name);
        }

        await _gameRepository.CreateGameAsync(gameEntity);
    }

    public async Task<bool> DeleteByKeyAsync(string key)
    {
        logger.LogTrace(nameof(this.DeleteByKeyAsync));

        return await _gameRepository.DeleteByKeyAsync(key);
    }

    public async Task<int> GetTotalGamesCountAsync()
    {
        logger.LogTrace(nameof(this.GetTotalGamesCountAsync));

        return await _gameRepository.GetTotalGamesCountAsync();
    }

    private static async Task ValidateGamestoreEntitiesExistAsync(IEnumerable<Guid> ids, Func<Guid, Task<bool>> isExistFunc, string entityName)
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

    private async Task ValidatePublisherExistAsync(Guid id)
    {
        var exist = await _publisherRepository.PublisherExistAsync(id);

        if (!exist)
        {
            throw new NotFoundException($"Publisher with ID {id} does not exist.");
        }
    }
}