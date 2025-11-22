using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.WebApi.Exceptions;
using Gamestore.WebApi.Models.Models.DTO;
using Gamestore.WebApi.Services.Interfaces;

namespace Gamestore.WebApi.Services;

public class GameDatabaseService(IGameRepository gameRepository,
    IGenreRepository genreRepository, IPlatformRepository platformRepository, IKeyGenerator uniqueKeyGenerator,
    IMapper mapper) : IGameDatabaseService
{
    private readonly IGameRepository _gameRepository = gameRepository;
    private readonly IGenreRepository _genreRepository = genreRepository;
    private readonly IPlatformRepository _platformRepository = platformRepository;
    private readonly IKeyGenerator _uniqueKeyGenerator = uniqueKeyGenerator;
    private readonly IMapper _mapper = mapper;

    public async Task UpdateGameAsync(GameUpdateExtendedDto model)
    {
        var entity = await _gameRepository.GetGameWithJoinsAsync(model.Game.Id) ?? throw new NotFoundException($"Game with ID {model.Game.Id} does not exist.");
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
        entity.GameGenres = [.. model.Genres!.Distinct().Select(id => new GameGenreEntity { GenreId = id })];
        entity.GamePlatforms = [.. model.Platforms!.Distinct().Select(id => new GamePlatformEntity { PlatformId = id })];
        await _gameRepository.SaveChangesAsync();
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
        var gameEntity = _mapper.Map<GameEntity>(game);

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

    private static async Task ValidateEntitiesExistAsync(IEnumerable<Guid> ids, Func<Guid, Task<bool>> isExistFunc, string entityName)
    {
        foreach (var id in ids.Distinct())
        {
            if (!await isExistFunc(id))
            {
                throw new NotFoundException($"{entityName} with ID {id} does not exist.");
            }
        }
    }
}
