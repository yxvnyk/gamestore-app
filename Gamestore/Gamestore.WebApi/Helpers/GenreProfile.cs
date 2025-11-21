using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.WebApi.Models.Models.DTO;

namespace Gamestore.WebApi.Helpers;

public class GenreProfile : Profile
{
    public GenreProfile()
    {
        _ = CreateMap<GenreDtoCreate, GenreEntity>();
        _ = CreateMap<GenreEntity, GenreDto>();
        _ = CreateMap<GenreEntity, GenreFullDto>();
    }
}
