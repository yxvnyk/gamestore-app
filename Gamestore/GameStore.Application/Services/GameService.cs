using AutoMapper;
using GameStore.Application.Helpers.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models.DTO;

namespace Gamestore.Application.Services;

public class GameService(IGameRepository gameRepository,
    IGenreRepository genreRepository, IPlatformRepository platformRepository, IKeyGenerator uniqueKeyGenerator,
    IMapper mapper) : Interfaces.IGameService
{
    private readonly IGameRepository _gameRepository = gameRepository;
    private readonly IGenreRepository _genreRepository = genreRepository;
    private readonly IPlatformRepository _platformRepository = platformRepository;
    private readonly IKeyGenerator _uniqueKeyGenerator = uniqueKeyGenerator;
    private readonly IMapper _mapper = mapper;

    public async Task UpdateGameAsync(GameUpdateExtendedDto model)
    {
        var entity = await _gameRepository.GetGameWithJoinsAsync(model.Game.Id)
            ?? throw new NotFoundException($"Game with ID {model.Game.Id} does not exist.");
        entity.GameGenres.Clear();
        entity.GamePlatforms.Clear();

        if (model.Game is not null)
        {
            await ValidateEntitiesExistAsync(model.Genres!, _genreRepository.GenreExistsAsync, "Genre");
        }

        if (model.Platforms is not null)
        {
            await ValidateEntitiesExistAsync(model.Platforms, _platformRepository.PlatformExistsAsync, "Platform");
        }

        _mapper.Map(model, entity);
        entity.GameGenres = [.. model.Genres!.Distinct().Select(id => new GameGenre { GenreId = id })];
        entity.GamePlatforms = [.. model.Platforms!.Distinct().Select(id => new GamePlatform { PlatformId = id })];
        await _gameRepository.UpdateGameAsync(entity);
    }

    public async Task<GameDto> GetGameAsync(string key)
    {
        var gameEntity = await _gameRepository.GetGameByKeyAsync(key);
        return gameEntity is not null ? _mapper.Map<GameDto>(gameEntity) : throw new NotFoundException($"Game with key '{key}' not found.");
    }

    public async Task<GameDto> GetGameAsync(Guid id)
    {
        var gameEntity = await _gameRepository.GetGameByIdAsync(id);
        return gameEntity is not null ? _mapper.Map<GameDto>(gameEntity) : throw new NotFoundException($"Game with ID '{id}' not found.");
    }

    public async Task<ICollection<GameDto>> GetGamesByPlatformAsync(Guid id)
    {
        var gameEntities = await _gameRepository.GetGamesByPlatformAsync(id);
        var gameDtos = gameEntities.Select(_mapper.Map<GameDto>).ToList();
        return gameDtos;
    }

    public async Task<ICollection<GameDto>> GetGamesByGenreAsync(Guid id)
    {
        var gameEntities = await _gameRepository.GetGamesByGenreAsync(id);
        var gameDtos = gameEntities.Select(_mapper.Map<GameDto>).ToList();
        return gameDtos;
    }

    public async Task<ICollection<GameDto>> GetAllGamesAsync()
    {
        var gameEntities = await _gameRepository.GetAllGamesAsync();
        var gameDtos = gameEntities.Select(_mapper.Map<GameDto>).ToList();
        return gameDtos;
    }

    public async Task CreateGameAsync(GameCreateExtendedDto game)
    {
        await ValidateEntitiesExistAsync(game.Genres, _genreRepository.GenreExistsAsync, "Genre");
        await ValidateEntitiesExistAsync(game.Platforms, _platformRepository.PlatformExistsAsync, "Platform");

        game.Genres = [.. game.Genres.Distinct()];
        game.Platforms = [.. game.Platforms.Distinct()];
        var gameEntity = _mapper.Map<Game>(game);

        if (string.IsNullOrWhiteSpace(gameEntity.Key))
        {
            gameEntity.Key = await _uniqueKeyGenerator.GenerateUniqueKeyAsync(gameEntity.Name);
        }

        await _gameRepository.CreateGameAsync(gameEntity);
    }

    public async Task<bool> DeleteByKeyAsync(string key)
    {
        return await _gameRepository.DeleteByKeyAsync(key);
    }

    public async Task<int> GetTotalGamesCountAsync()
    {
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
}
