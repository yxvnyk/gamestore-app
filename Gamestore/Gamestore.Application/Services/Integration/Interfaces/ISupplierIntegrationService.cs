using Gamestore.Domain.Models.DTO.Publisher;

namespace Gamestore.Application.Services.Integration.Interfaces;

public interface ISupplierIntegrationService
{
    Task PromoteToPublisherAndUpdateAsync(PublisherUpdateDto updateDto);

    Task<Guid> EnsurePromotedAsync(int id);
}
