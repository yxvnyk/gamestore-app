using AutoMapper;
using Gamestore.Domain.Helpers;
using Gamestore.Domain.Models.DTO.Game;

namespace Gamestore.Domain.Profiles;

public class GetGamesRequestProfile : Profile
{
    public GetGamesRequestProfile()
    {
        CreateMap<GetGamesApiRequest, GetGamesRequest>()
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src =>
                PaginationOptionsHelper.GetValidPageSize(src.PageCount)))
            .ForMember(dest => dest.Page, opt => opt.MapFrom(src =>
                PaginationOptionsHelper.GetValidCurrentPage(src.Page)))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.MinPrice, opt => opt.MapFrom(src => src.MinPrice))
            .ForMember(dest => dest.MaxPrice, opt => opt.MapFrom(src => src.MaxPrice))
            .ForMember(dest => dest.DatePublishing, opt => opt.MapFrom(src => src.DatePublishing))
            .ForMember(dest => dest.Sort, opt => opt.MapFrom(src => src.Sort))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres))
            .ForMember(dest => dest.Platforms, opt => opt.MapFrom(src => src.Platforms))
            .ForMember(dest => dest.Publishers, opt => opt.MapFrom(src => src.Publishers));
    }
}