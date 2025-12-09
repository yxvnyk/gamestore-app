using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Models.DTO;

namespace Gamestore.Application.Helpers.Profiles;

public class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        _ = CreateMap<PlatformDto, Platform>();
        _ = CreateMap<Platform, PlatformDto>();

        _ = CreateMap<Platform, PlatformFullDto>();
        _ = CreateMap<PlatformUpdateDto, Platform>();
    }
}
