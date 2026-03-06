using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Northwind.Entities;
using Gamestore.Domain.Models.DTO.Publisher;

namespace Gamestore.Application.Helpers.Profiles;

public class PublisherProfile : Profile
{
    public PublisherProfile()
    {
        CreateMap<PublisherCreateDto, Publisher>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Games, opt => opt.Ignore());

        CreateMap<PublisherUpdateDto, Publisher>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Games, opt => opt.Ignore());

        CreateMap<Publisher, PublisherDto>();

        CreateMap<Supplier, PublisherDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SupplierId.ToString()))
            .ForMember(dest => dest.HomePage, opt => opt.MapFrom(src => src.HomePage))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
            .ForMember(dest => dest.Description, opt => opt.Ignore());
    }
}
