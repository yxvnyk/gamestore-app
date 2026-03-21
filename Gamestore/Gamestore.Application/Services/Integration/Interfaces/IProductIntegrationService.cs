namespace Gamestore.Application.Services.Integration.Interfaces;

public interface IProductIntegrationService
{
    Task<Guid> EnsurePromotedAsync(int id);
}
