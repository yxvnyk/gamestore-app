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
    public async Task CreatePublisherAsync(PublisherDto publisher)
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
}
