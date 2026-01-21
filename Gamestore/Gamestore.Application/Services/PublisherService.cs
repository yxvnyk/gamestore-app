using AutoMapper;
using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models.DTO.Publisher;
using Microsoft.Extensions.Logging;

namespace Gamestore.Application.Services;

public class PublisherService(IPublisherRepository publisherRepository,
    IMapper mapper, ILogger<PublisherService> logger) : IPublisherService
{
    public async Task CreatePublisherAsync(PublisherCreateDto publisher)
    {
        logger.LogTrace(nameof(this.CreatePublisherAsync));

        var uniqueName = await publisherRepository.PublisherCompanyNameExistAsync(publisher.CompanyName);
        if (uniqueName)
        {
            throw new NotUniqueCompanyNameException();
        }

        var gameEntity = mapper.Map<Publisher>(publisher);
        await publisherRepository.CreatePublisherAsync(gameEntity);
    }

    public async Task UpdatePublisherAsync(PublisherUpdateDto publisher)
    {
        logger.LogTrace(nameof(this.CreatePublisherAsync));

        var entity = await publisherRepository.GetPublisherByIdAsync(publisher.Id) ?? throw new NotFoundException("Publisher not found");
        await VerifyCompanyName(publisher.CompanyName);

        mapper.Map(publisher, entity);
        await publisherRepository.UpdatePublisherAsync(entity);
    }

    public async Task<bool> DeletePublisherAsync(Guid id)
    {
        return await publisherRepository.DeletePublisherAsync(id);
    }

    public async Task<PublisherDto> GetPublisherByCompanyNameAsync(string companyName)
    {
        logger.LogTrace(nameof(this.GetPublisherByCompanyNameAsync));

        var publisher = await publisherRepository.GetPublisherByCompanyNameAsync(companyName);
        return mapper.Map<PublisherDto>(publisher);
    }

    public async Task<PublisherDto?> GetPublisherByGameKeyAsync(string key)
    {
        logger.LogTrace(nameof(this.GetPublisherByGameKeyAsync));

        var publisher = await publisherRepository.GetPublisherByGameKeyAsync(key);
        return mapper.Map<PublisherDto>(publisher);
    }

    public async Task<IEnumerable<PublisherDto>> GetAllPublishersAsync()
    {
        logger.LogTrace(nameof(this.GetAllPublishersAsync));

        var publishers = await publisherRepository.GetAllPublishersAsync();
        return [.. publishers.Select(mapper.Map<PublisherDto>)];
    }

    public async Task VerifyCompanyName(string? companyName)
    {
        if (companyName is null)
        {
            return;
        }

        var uniqueName = await publisherRepository.PublisherCompanyNameExistAsync(companyName);
        if (uniqueName)
        {
            throw new NotUniqueCompanyNameException();
        }
    }
}
