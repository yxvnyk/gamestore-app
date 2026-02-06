using AutoMapper;
using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Extensions;
using Gamestore.Domain.Models.DTO.Platform;
using Microsoft.Extensions.Logging;

namespace Gamestore.Application.Services;

public class PlatformService(IPlatformRepository platformRepository,
    IGameRepository gameRepository, IMapper mapper, ILogger<PlatformService> logger) : IPlatformService
{
    public async Task CreatePlatformAsync(PlatformCreateDto model)
    {
        logger.LogTrace(nameof(this.CreatePlatformAsync));

        var entity = mapper.Map<PlatformCreateDto, Platform>(model);
        await platformRepository.CreatePlatformAsync(entity);
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        logger.LogTrace(nameof(this.DeleteByIdAsync));

        return await platformRepository.DeleteByIdAsync(id);
    }

    public async Task<IEnumerable<PlatformDto>> GetAllPlatformsAsync()
    {
        logger.LogTrace(nameof(this.GetAllPlatformsAsync));

        var genreEntities = await platformRepository.GetAllPlatformsAsync();
        return [.. genreEntities.Select(mapper.Map<PlatformDto>)];
    }

    public async Task<PlatformDto?> GetPlatformByIdAsync(Guid id)
    {
        logger.LogTrace(nameof(this.GetPlatformByIdAsync));

        var platformEntity = await platformRepository.GetPlatformByIdAsync(id);
        return platformEntity is not null ? mapper.Map<PlatformDto>(platformEntity) : throw new NotFoundException($"Platform with ID {id} does not exist.");
    }

    public async Task<IEnumerable<PlatformDto>> GetPlatformsByGameKeyAsync(string key)
    {
        logger.LogTrace(nameof(this.GetPlatformsByGameKeyAsync));

        var gameExists = await gameRepository.GameKeyExistAsync(key);
        if (!gameExists)
        {
            throw new NotFoundException($"Game with key {key} not found.");
        }

        var platformEntities = await platformRepository.GetPlatformsByGameKeyAsync(key);
        return [.. platformEntities.Select(mapper.Map<PlatformDto>)];
    }

    public async Task UpdatePlatformAsync(PlatformUpdateDto model)
    {
        logger.LogTrace(nameof(this.UpdatePlatformAsync));

        var entity = await platformRepository.GetPlatformByIdAsync(model.Id) ?? throw new NotFoundException($"Platform with ID {model.Id} does not exist.");
        mapper.Map(model, entity);
        await platformRepository.UpdatePlatformAsync(entity);
    }
}
