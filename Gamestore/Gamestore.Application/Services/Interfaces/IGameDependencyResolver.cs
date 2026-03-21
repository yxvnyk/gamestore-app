using Gamestore.Domain.Models;

namespace Gamestore.Application.Services.Interfaces;

public interface IGameDependencyResolver
{
    Task<IEnumerable<Guid>> ResolveAndValidateGenresAsync(IEnumerable<Identity> ids);

    Task<Guid> ResolveAndValidatePublisherAsync(Identity id);

    Task ValidatePlatformsExistAsync(IEnumerable<Guid> ids);

    Task<Guid> GetGameGuidOrPromote(Identity id);
}
