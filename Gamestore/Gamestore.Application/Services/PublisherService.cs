using AutoMapper;
using Gamestore.Application.Services.Integration.Interfaces;
using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Northwind.Entities;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models;
using Gamestore.Domain.Models.DTO.Publisher;
using Microsoft.Extensions.Logging;

namespace Gamestore.Application.Services;

public class PublisherService(IPublisherRepository publisherRepository,
    INorthwindSupplierRepository northwindSupplierRepository,
    ISupplierIntegrationService supplierIntegrationService,
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

    public async Task UpdatePublisherAsync(PublisherUpdateDto updateDto)
    {
        logger.LogTrace(nameof(this.CreatePublisherAsync));

        await VerifyCompanyName(updateDto.CompanyName);
        if (updateDto.Id.IsGuid)
        {
            var entity = await publisherRepository.GetPublisherByIdAsync(updateDto.Id.GuidId!.Value) ?? throw new NotFoundException("Publisher not found");

            mapper.Map(updateDto, entity);
            await publisherRepository.UpdatePublisherAsync(entity);
        }

        await supplierIntegrationService.PromoteToPublisherAndUpdateAsync(updateDto);
    }

    public async Task<bool> DeletePublisherAsync(Identity identity)
    {
        return identity.IsInt
            ? throw new BusinessRuleValidationException()
            : await publisherRepository.DeletePublisherAsync(identity.GuidId!.Value);
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
            return mapper.Map<PublisherDto>(supplier);
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

        var publishersDtos = RemoveGenreDuplications(publishers, suppliers);

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

    private IEnumerable<PublisherDto> RemoveGenreDuplications(IEnumerable<Publisher> publisher, IEnumerable<Supplier> supplier)
    {
        var publisherLegacyIds = publisher.Select(g => g.LegacyId).ToHashSet();
        var uniqueSuppliers = supplier.Where(p => !publisherLegacyIds.Contains(p.SupplierId))
            .ToList();

        var genreDtos = mapper.Map<IEnumerable<PublisherDto>>(publisher);
        var combinedList = genreDtos.Concat(mapper.Map<IEnumerable<PublisherDto>>(uniqueSuppliers));
        return combinedList;
    }
}
