using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.WebApi.Models.Models.DTO;
using Gamestore.WebApi.Services.Interfaces;

namespace Gamestore.WebApi.Services;

public class PlatformDatabaseService(IPlatformRepository platformRepository, IGameRepository gameRepository, IMapper mapper) : IPlatformDatabaseService
{
    public async Task CreatePlatformAsync(PlatformDto model)
    {
        var entity = mapper.Map<PlatformDto, PlatformEntity>(model);
        await platformRepository.CreatePlatformAsync(entity);
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        return await platformRepository.DeleteByIdAsync(id);
    }

    public async Task<IEnumerable<PlatformFullDto>> GetAllPlatformsAsync()
    {
        var genreEntities = await platformRepository.GetAllPlatformsAsync();
        return [.. genreEntities.Select(mapper.Map<PlatformFullDto>)];
    }

    public async Task<PlatformFullDto?> GetPlatformByIdAsync(Guid id)
    {
        var platformEntity = await platformRepository.GetPlatformByIdAsync(id);
        return platformEntity is not null ? mapper.Map<PlatformFullDto>(platformEntity) : null;
    }

    public async Task<IEnumerable<PlatformFullDto>> GetPlatformsByGameKeyAsync(string key)
    {
        var gameExists = await gameRepository.GameKeyExistAsync(key);
        if (!gameExists)
        {
            throw new NotImplementedException($"Game with key '{key}' not found.");
        }

        var platformEntities = await platformRepository.GetPlatformsByGameKeyAsync(key);
        return [.. platformEntities.Select(mapper.Map<PlatformFullDto>)];
    }

    public async Task UpdatePlatformAsync(PlatformUpdateDto model)
    {
        var entity = await platformRepository.GetPlatformByIdAsync(model.Id) ?? throw new ArgumentException($"Game with ID {model.Id} does not exist.");
        mapper.Map(model, entity);
        await platformRepository.SaveChangesAsync();
    }
}
