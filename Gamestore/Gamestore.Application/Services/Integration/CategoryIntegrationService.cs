using AutoMapper;
using Gamestore.Application.Services.Integration.Interfaces;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Models.DTO.Genre;

namespace Gamestore.Application.Services.Integration;

public class CategoryIntegrationService(INorthwindCategoryRepository categoryRepository, IGenreRepository genreRepository, IMapper mapper) : ICategoryIntegrationService
{
    public async Task PromoteToGenreAndUpdateAsync(GenreUpdateDto updateDto)
    {
        var id = updateDto.Id.IntId!.Value;
        var category = await categoryRepository.GetAsync(id) ?? throw new NotFoundException($"Category with ID {id} does not exist.");
        var genre = mapper.Map<Genre>(category);
        mapper.Map(updateDto, genre);

        await genreRepository.CreateGenreAsync(genre);
    }

    public async Task<Guid> EnsurePromotedAsync(int id)
    {
        var category = await categoryRepository.GetAsync(id) ?? throw new NotFoundException($"Category with ID {id} does not exist.");

        var promotedId = await genreRepository.GetIdByLegacyIdAsync(category.CategoryId);
        if (promotedId is not null && promotedId != Guid.Empty)
        {
            return promotedId!.Value;
        }

        var genre = mapper.Map<Genre>(category);
        await genreRepository.CreateGenreAsync(genre);
        return genre.Id;
    }
}
