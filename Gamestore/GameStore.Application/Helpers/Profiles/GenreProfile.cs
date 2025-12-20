using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Models.DTO;

namespace Gamestore.Application.Helpers.Profiles;

public class GenreProfile : Profile
{
    public GenreProfile()
    {
        _ = CreateMap<GenreCreateDto, Genre>()
            .ForMember(dest => dest.GameGenres, opt => opt.Ignore())
            .ForMember(dest => dest.ParentGenre, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        _ = CreateMap<Genre, GenreDto>();
        _ = CreateMap<Genre, GenreFullDto>();
        _ = CreateMap<GenreUpdateDto, Genre>()
            .ForMember(dest => dest.GameGenres, opt => opt.Ignore())
            .ForMember(dest => dest.ParentGenre, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
