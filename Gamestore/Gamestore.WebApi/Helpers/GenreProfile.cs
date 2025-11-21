using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.WebApi.Models.Models.DTO;

namespace Gamestore.WebApi.Helpers;

public class GenreProfile : Profile
{
    public GenreProfile()
    {
        _ = CreateMap<GenreCreateDto, GenreEntity>();
        _ = CreateMap<GenreEntity, GenreDto>();
        _ = CreateMap<GenreEntity, GenreFullDto>();
        _ = CreateMap<GenreUpdateDto, GenreEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
