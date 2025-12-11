using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Models.DTO;

namespace Gamestore.Application.Helpers.Profiles;

public class GameProfile : Profile
{
    public GameProfile()
    {
        CreateMap<GameCreateExtendedDto, Game>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Game.Name))
            .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Game.Key))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Game.Description))
            .ForMember(dest => dest.GameGenres, opt => opt.MapFrom(src =>
                src.Genres.Select(id => new GameGenre { GenreId = id })))
            .ForMember(dest => dest.GamePlatforms, opt => opt.MapFrom(src =>
                src.Platforms.Select(id => new GamePlatform { PlatformId = id })))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        _ = CreateMap<Game, GameDto>();

        CreateMap<GameUpdateExtendedDto, Game>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Game.Name))
            .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Game.Key))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Game.Description))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        _ = CreateMap<Game, GameDto>();
    }
}
