using Gamestore.Domain.Models.DTO.Genre;

namespace Gamestore.Application.Services.Integration.Interfaces;

public interface ICategoryIntegrationService
{
    Task PromoteToGenreAndUpdateAsync(GenreUpdateDto updateDto);
}
