using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Northwind.Entities;
using Gamestore.DataAccess.Wrappers;
using Gamestore.Domain.Models.DTO.Game;

namespace Gamestore.Application.Helpers.Profiles;

public class GameProfile : Profile
{
    public GameProfile()
    {
        CreateMap<CreateGameRequest, Game>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.Publisher, opt => opt.Ignore())
            .ForMember(dest => dest.Comments, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Game.Name))
            .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Game.Key))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Game.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Game.Price))
            .ForMember(dest => dest.UnitsInStock, opt => opt.MapFrom(src => src.Game.UnitInStock))
            .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Game.Discount))
            .ForMember(dest => dest.GameGenres, opt => opt.MapFrom(src =>
                src.Genres.Select(id => new GameGenre { GenreId = id })))
            .ForMember(dest => dest.GamePlatforms, opt => opt.MapFrom(src =>
                src.Platforms.Select(id => new GamePlatform { PlatformId = id })))
            .ForMember(dest => dest.PublisherId, opt => opt.MapFrom(src => src.Publisher))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        _ = CreateMap<Game, GameDto>();

        _ = CreateMap<Product, GameDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.GameKey))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.QuantityPerUnit))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.UnitInStock, opt => opt.MapFrom(src => src.UnitsInStock))
            .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discontinued))
            .ForMember(dest => dest.CommentCount, opt => opt.Ignore());

        CreateMap<Product, Game>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.GameKey))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => (double)src.UnitPrice))
            .ForMember(dest => dest.UnitsInStock, opt => opt.MapFrom(src => src.UnitsInStock))
            .ForMember(dest => dest.QuantityPerUnit, opt => opt.MapFrom(src => src.QuantityPerUnit))
            .ForMember(dest => dest.UnitsOnOrder, opt => opt.MapFrom(src => src.UnitsOnOrder))
            .ForMember(dest => dest.ReorderLevel, opt => opt.MapFrom(src => src.ReorderLevel))
            .ForMember(dest => dest.Discontinued, opt => opt.MapFrom(src => src.Discontinued))
            .ForMember(dest => dest.PublisherId, opt => opt.Ignore())
            .ForMember(dest => dest.GameGenres, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Description, opt => opt.Ignore())
            .ForMember(dest => dest.Discount, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Publisher, opt => opt.Ignore())
            .ForMember(dest => dest.GamePlatforms, opt => opt.Ignore())
            .ForMember(dest => dest.Comments, opt => opt.Ignore());

        _ = CreateMap<GameWithStats, GameDto>()
            .IncludeMembers(src => src.Game)
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.Game.CreatedDate))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.CommentCount));

        CreateMap<UpdateGameRequest, Game>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.Publisher, opt => opt.Ignore())
            .ForMember(dest => dest.Comments, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Game.Name))
            .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Game.Key))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Game.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Game.Price))
            .ForMember(dest => dest.UnitsInStock, opt => opt.MapFrom(src => src.Game.UnitInStock))
            .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Game.Discount))
            .ForMember(dest => dest.GameGenres, opt => opt.Ignore())
            .ForMember(dest => dest.GamePlatforms, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
