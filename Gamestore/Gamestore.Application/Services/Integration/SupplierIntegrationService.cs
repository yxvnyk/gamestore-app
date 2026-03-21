using AutoMapper;
using Gamestore.Application.Services.Integration.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models.DTO.Publisher;

namespace Gamestore.Application.Services.Integration;

public class SupplierIntegrationService(INorthwindSupplierRepository supplierRepository, IPublisherRepository publisherRepository, IMapper mapper) : ISupplierIntegrationService
{
    public async Task PromoteToPublisherAndUpdateAsync(PublisherUpdateDto updateDto)
    {
        var id = updateDto.Id.IntId!.Value;
        var supplier = await supplierRepository.GetAsync(id) ?? throw new NotFoundException($"Supplier with ID {id} does not exist.");
        var publisher = mapper.Map<Publisher>(supplier);
        mapper.Map(updateDto, publisher);

        await publisherRepository.CreatePublisherAsync(publisher);
    }

    public async Task<Guid> EnsurePromotedAsync(int id)
    {
        var supplier = await supplierRepository.GetAsync(id) ?? throw new NotFoundException($"Supplier with ID {id} does not exist.");

        var promotedId = await publisherRepository.GetIdByLegacyIdAsync(supplier.SupplierId);
        if (promotedId is not null && promotedId != Guid.Empty)
        {
            return promotedId.Value;
        }

        var publisher = mapper.Map<Publisher>(supplier);
        await publisherRepository.CreatePublisherAsync(publisher);
        return publisher.Id;
    }
}
