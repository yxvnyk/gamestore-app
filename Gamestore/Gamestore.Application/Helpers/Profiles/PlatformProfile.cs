using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Models.DTO.Platform;

namespace Gamestore.Application.Helpers.Profiles;

public class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        _ = CreateMap<PlatformCreateDto, Platform>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.GamePlatforms, opt => opt.Ignore());

        _ = CreateMap<Platform, PlatformCreateDto>();

        _ = CreateMap<Platform, PlatformDto>();
        _ = CreateMap<PlatformUpdateDto, Platform>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.GamePlatforms, opt => opt.Ignore());
    }
}
