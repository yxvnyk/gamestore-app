using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.WebApi.Models.Models.DTO;

namespace Gamestore.WebApi.Helpers;

public class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        _ = CreateMap<PlatformDto, PlatformEntity>();
        _ = CreateMap<PlatformEntity, PlatformDto>();

        _ = CreateMap<PlatformEntity, PlatformFullDto>();
    }
}
