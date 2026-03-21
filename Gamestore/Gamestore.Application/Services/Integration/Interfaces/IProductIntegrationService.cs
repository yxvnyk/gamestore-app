using Gamestore.Domain.Models;

namespace Gamestore.Application.Services.Integration.Interfaces;

public interface IProductIntegrationService
{
    Task<Guid> EnsurePromotedAsync(Identity identity);
}
