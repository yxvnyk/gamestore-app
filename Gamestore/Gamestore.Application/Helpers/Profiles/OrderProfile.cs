using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Models.DTO.Order;
using NorthwindOrder = Gamestore.DataAccess.Northwind.Entities.Order;

namespace Gamestore.Application.Helpers.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>();

        CreateMap<NorthwindOrder, OrderDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.OrderDate));
    }
}
