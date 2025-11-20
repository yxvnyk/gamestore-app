using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.WebApi.Models.Models.DTO;
using Gamestore.WebApi.Services.Interfaces;

namespace Gamestore.WebApi.Services;

public class GenreDatabaseService(IGenreRepository genreRepository, IMapper mapper) : IGenreDatabaseService
{
    public async Task CreateGenreAsync(GenreDto genre)
    {
        if (genre.ParentGenreId != null)
        {
            if (!await genreRepository.GenreExistsAsync(genre.ParentGenreId.Value))
            {
                throw new ArgumentException($"Parent genre with ID {genre.ParentGenreId} does not exist.");
            }
        }

        var enity = mapper.Map<GenreDto, GenreEntity>(genre);
        await genreRepository.CreateGenreAsync(enity);
    }
}
