using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Northwind.Entities;
using Gamestore.Domain.Models.DTO.Genre;

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

        _ = CreateMap<Category, GenreDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId));

        _ = CreateMap<Category, Genre>()
            .ForMember(dest => dest.LegacyId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Picture, opt => opt.MapFrom(src => src.Picture))
            .ForMember(dest => dest.ParentGenreId, opt => opt.Ignore())
            .ForMember(dest => dest.ParentGenre, opt => opt.Ignore())
            .ForMember(dest => dest.GameGenres, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        _ = CreateMap<Category, GenreFullDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ParentGenreId, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId));

        _ = CreateMap<GenreUpdateDto, Genre>()
            .ForMember(dest => dest.GameGenres, opt => opt.Ignore())
            .ForMember(dest => dest.ParentGenre, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
