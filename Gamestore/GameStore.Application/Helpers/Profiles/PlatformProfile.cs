using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Models.DTO;

namespace Gamestore.Application.Helpers.Profiles;

public class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        _ = CreateMap<PlatformDto, Platform>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.GamePlatforms, opt => opt.Ignore());

        _ = CreateMap<Platform, PlatformDto>();

        _ = CreateMap<Platform, PlatformFullDto>();
        _ = CreateMap<PlatformUpdateDto, Platform>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.GamePlatforms, opt => opt.Ignore());
    }
}
