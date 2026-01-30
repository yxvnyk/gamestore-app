using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Models.DTO.Cart;

namespace Gamestore.Application.Helpers.Profiles;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<OrderGame, CartDto>();
    }
}
