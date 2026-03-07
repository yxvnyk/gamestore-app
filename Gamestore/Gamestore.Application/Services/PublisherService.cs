using AutoMapper;
using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models.DTO.Publisher;
using Microsoft.Extensions.Logging;

namespace Gamestore.Application.Services;

public class PublisherService(IPublisherRepository publisherRepository,
    INorthwindSupplierRepository northwindSupplierRepository,
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

    public async Task<PublisherDto?> GetPublisherByCompanyNameAsync(string companyName)
    {
        logger.LogTrace(nameof(this.GetPublisherByCompanyNameAsync));

        var publisher = await publisherRepository.GetPublisherByCompanyNameAsync(companyName);
        if (publisher != null)
        {
            return mapper.Map<PublisherDto>(publisher);
        }

        var supplier = await northwindSupplierRepository.GetByCompanyNameAsync(companyName);

        if (supplier != null)
        {
            return mapper.Map<PublisherDto>(supplier);
        }

        // null
        return null;
    }

    public async Task<PublisherDto?> GetPublisherByGameKeyAsync(string key)
    {
        logger.LogTrace(nameof(this.GetPublisherByGameKeyAsync));

        var publisher = await publisherRepository.GetPublisherByGameKeyAsync(key);
        if (publisher != null)
        {
            return mapper.Map<PublisherDto>(publisher);
        }

        var supplier = await northwindSupplierRepository.GetByGameKeyAsync(key);
        if (supplier != null)
        {
            return mapper.Map<PublisherDto>(publisher);
        }

        // null
        return null;
    }

    public async Task<IEnumerable<PublisherDto>> GetAllPublishersAsync()
    {
        logger.LogTrace(nameof(this.GetAllPublishersAsync));

        var publisherTask = publisherRepository.GetAllPublishersAsync();
        var supplierTask = northwindSupplierRepository.GetAllAsync();

        await Task.WhenAll(publisherTask, supplierTask);

        var publishers = await publisherTask;
        var suppliers = await supplierTask;

        var publishersDtos = mapper.Map<IEnumerable<PublisherDto>>(publishers);
        publishersDtos = publishersDtos.Concat(mapper.Map<IEnumerable<PublisherDto>>(suppliers));

        return publishersDtos;
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
