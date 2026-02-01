using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Models.DTO.OrderItem;

namespace Gamestore.Application.Helpers.Profiles;

public class OrderItemProfile : Profile
{
    public OrderItemProfile()
    {
        CreateMap<OrderGame, OrderItemDto>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));
    }
}
