using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.WebApi.Models.Models.DTO;
using Gamestore.WebApi.Services.Interfaces;

namespace Gamestore.WebApi.Services;

public class GameDatabaseService(IGameRepository gameRepository,
    IGenreRepository genreRepository, IPlatformRepository platformRepository, UniqueKeyGenerator uniqueKeyGenerator,
    IMapper mapper) : IGameDatabaseService
{
    private readonly IGameRepository _gameRepository = gameRepository;
    private readonly IGenreRepository _genreRepository = genreRepository;
    private readonly IPlatformRepository _platformRepository = platformRepository;
    private readonly UniqueKeyGenerator _uniqueKeyGenerator = uniqueKeyGenerator;
    private readonly IMapper _mapper = mapper;

    public async Task<bool> UpdateGameAsync(GameUpdateExtendedDto model)
    {
        var entity = await _gameRepository.GetGameWithJoinsAsync(model.Game.Id);
        if (entity is null)
        {
            return false;
        }

        entity.GameGenres.Clear();
        entity.GamePlatforms.Clear();

        foreach (var genreId in model.Genres)
        {
            var genreExists = await _genreRepository.GenreExistsAsync(genreId);
            if (!genreExists)
            {
                throw new ArgumentException($"Genre with ID {genreId} does not exist.");
            }
        }

        foreach (var platformId in model.Platforms)
        {
            var platformExists = await _platformRepository.PlatformExistsAsync(platformId);
            if (!platformExists)
            {
                throw new ArgumentException($"Platform with ID {platformId} does not exist.");
            }
        }

        _mapper.Map(model, entity);
        entity.GameGenres = [.. model.Genres.Distinct().Select(id => new GameGenreEntity { GenreId = id })];
        entity.GamePlatforms = [.. model.Platforms.Distinct().Select(id => new GamePlatformEntity { PlatformId = id })];
        await _gameRepository.SaveChangesAsync();
        return true;
    }

    public async Task<GameDto> GetGameAsync(string key)
    {
        var gameEntity = await _gameRepository.GetGameByKeyAsync(key);
        return gameEntity is not null ? _mapper.Map<GameDto>(gameEntity) : null;
    }

    public async Task<GameDto> GetGameAsync(Guid id)
    {
        var gameEntity = await _gameRepository.GetGameByIdAsync(id);
        return gameEntity is not null ? _mapper.Map<GameDto>(gameEntity) : null;
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

    public async Task CreateGameAsync(GameCreateExtendedDto game)
    {
        foreach (var genreId in game.Genres)
        {
            var genreExists = await _genreRepository.GenreExistsAsync(genreId);
            if (!genreExists)
            {
                throw new ArgumentException($"Genre with ID {genreId} does not exist.");
            }
        }

        foreach (var platformId in game.Platforms)
        {
            var platformExists = await _platformRepository.PlatformExistsAsync(platformId);
            if (!platformExists)
            {
                throw new ArgumentException($"Platform with ID {platformId} does not exist.");
            }
        }

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
}
