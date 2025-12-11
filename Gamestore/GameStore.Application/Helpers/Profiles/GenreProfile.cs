using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Models.DTO;

namespace Gamestore.Application.Helpers.Profiles;

public class GenreProfile : Profile
{
    public GenreProfile()
    {
        _ = CreateMap<GenreCreateDto, Genre>();
        _ = CreateMap<Genre, GenreDto>();
        _ = CreateMap<Genre, GenreFullDto>();
        _ = CreateMap<GenreUpdateDto, Genre>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
