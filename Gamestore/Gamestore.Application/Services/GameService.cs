using AutoMapper;
using GameStore.Application.Helpers.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Extensions;
using Gamestore.Domain.Models.DTO.Game;
using Microsoft.Extensions.Logging;

namespace Gamestore.Application.Services;

public class GameService(IGameRepository gameRepository,
    IGenreRepository genreRepository, IPlatformRepository platformRepository, IPublisherRepository publisherRepository,
    IKeyGenerator uniqueKeyGenerator,
    IMapper mapper, ILogger<GameService> logger) : Interfaces.IGameService
{
    private readonly IGameRepository _gameRepository = gameRepository;
    private readonly IGenreRepository _genreRepository = genreRepository;
    private readonly IPlatformRepository _platformRepository = platformRepository;
    private readonly IPublisherRepository _publisherRepository = publisherRepository;
    private readonly IKeyGenerator _uniqueKeyGenerator = uniqueKeyGenerator;
    private readonly IMapper _mapper = mapper;

    public async Task UpdateGameAsync(UpdateGameRequest updateRequest)
    {
        logger.LogTrace(nameof(this.UpdateGameAsync));

        var entity = await _gameRepository.GetGameWithJoinsAsync(updateRequest.Game.Id)
            ?? throw new NotFoundException($"Game with ID {updateRequest.Game.Id} does not exist.");
        entity.GameGenres.Clear();
        entity.GamePlatforms.Clear();

        if (updateRequest.Game is not null)
        {
            await ValidateEntitiesExistAsync(updateRequest.Genres!, _genreRepository.GenreExistsAsync, "Genre");
        }

        if (updateRequest.Platforms is not null)
        {
            await ValidateEntitiesExistAsync(updateRequest.Platforms, _platformRepository.PlatformExistsAsync, "Platform");
        }

        if (updateRequest.Publisher.HasValue)
        {
            await ValidatePublisherExistAsync(updateRequest.Publisher.Value);
        }

        _mapper.Map(updateRequest, entity);
        entity.GameGenres = [.. updateRequest.Genres!.Distinct().Select(id => new GameGenre { GenreId = id })];
        entity.GamePlatforms = [.. updateRequest.Platforms!.Distinct().Select(id => new GamePlatform { PlatformId = id })];
        await _gameRepository.UpdateGameAsync(entity);
    }

    public async Task<GameDto> GetGameAsync(string key)
    {
        logger.LogTrace(nameof(this.GetGameAsync));

        var gameEntity = await _gameRepository.GetGameByKeyAsync(key);
        return gameEntity is not null ? _mapper.Map<GameDto>(gameEntity) : throw new NotFoundException($"Game with key '{key}' not found.");
    }

    public async Task<GameDto> GetGameAsync(Guid id)
    {
        logger.LogTrace(nameof(this.GetGameAsync));

        var gameEntity = await _gameRepository.GetGameByIdAsync(id);
        return gameEntity is not null ? _mapper.Map<GameDto>(gameEntity) : throw new NotFoundException($"Game with ID '{id}' not found.");
    }

    public async Task<ICollection<GameDto>> GetGamesByPlatformAsync(Guid id)
    {
        logger.LogTrace(nameof(this.GetGamesByPlatformAsync));

        var gameEntities = await _gameRepository.GetGamesByPlatformAsync(id);
        var gameDtos = gameEntities.Select(_mapper.Map<GameDto>).ToList();
        return gameDtos;
    }

    public async Task<ICollection<GameDto>> GetGamesByGenreAsync(Guid id)
    {
        logger.LogTrace(nameof(this.GetGamesByGenreAsync));

        var gameEntities = await _gameRepository.GetGamesByGenreAsync(id);
        var gameDtos = gameEntities.Select(_mapper.Map<GameDto>).ToList();
        return gameDtos;
    }

    public async Task<ICollection<GameDto>> GetGamesByCompanyNameAsync(string companyName)
    {
        logger.LogTrace(nameof(this.GetGamesByCompanyNameAsync));

        var gameEntities = await _gameRepository.GetGamesByCompanyNameAsync(companyName);
        var gameDtos = gameEntities.Select(_mapper.Map<GameDto>).ToList();
        return gameDtos;
    }

    public async Task<ICollection<GameDto>> GetAllGamesAsync()
    {
        logger.LogTrace(nameof(this.GetAllGamesAsync));

        var gameEntities = await _gameRepository.GetAllGamesAsync();
        var gameDtos = gameEntities.Select(_mapper.Map<GameDto>).ToList();
        return gameDtos;
    }

    public async Task CreateGameAsync(CreateGameRequest createRequest)
    {
        logger.LogTrace(nameof(this.CreateGameAsync));

        await ValidateEntitiesExistAsync(createRequest.Genres, _genreRepository.GenreExistsAsync, "Genre");
        await ValidateEntitiesExistAsync(createRequest.Platforms, _platformRepository.PlatformExistsAsync, "Platform");
        await ValidatePublisherExistAsync(createRequest.Publisher);

        createRequest.Genres = [.. createRequest.Genres.Distinct()];
        createRequest.Platforms = [.. createRequest.Platforms.Distinct()];
        var gameEntity = _mapper.Map<Game>(createRequest);

        if (string.IsNullOrWhiteSpace(gameEntity.Key))
        {
            gameEntity.Key = await _uniqueKeyGenerator.GenerateUniqueKeyAsync(gameEntity.Name);
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

    private static async Task ValidateEntitiesExistAsync(IEnumerable<Guid> ids, Func<Guid, Task<bool>> isExistFunc, string entityName)
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
