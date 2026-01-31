using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Models.DTO.CartItem;

namespace Gamestore.Application.Helpers.Profiles;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<OrderGame, CartItemDto>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));
    }
}
