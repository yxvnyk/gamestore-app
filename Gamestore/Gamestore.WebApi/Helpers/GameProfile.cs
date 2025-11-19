using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.WebApi.Models.Models.DTO;

namespace Gamestore.WebApi.Helpers;

public class GameProfile : Profile
{
    public GameProfile()
    {
        _ = CreateMap<GameCreateDto, GameEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Game.Name))
            .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Game.Key))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Game.Description))
            .ForMember(dest => dest.GameGenres, opt => opt.MapFrom(src =>
                src.Genres.Select(id => new GameGenreEntity { GenreId = id })))
            .ForMember(dest => dest.GamePlatforms, opt => opt.MapFrom(src =>
                src.Platforms.Select(id => new GamePlatformEntity { PlatformId = id })));

        _ = CreateMap<GameEntity, GameDto>();
    }
}
