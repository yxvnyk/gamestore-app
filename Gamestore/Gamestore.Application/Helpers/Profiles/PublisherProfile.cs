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

        CreateMap<Supplier, Publisher>()
            .ForMember(dest => dest.LegacyId, opt => opt.MapFrom(src => src.SupplierId))

            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Description, opt => opt.Ignore())

            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
            .ForMember(dest => dest.ContactName, opt => opt.MapFrom(src => src.ContactName))
            .ForMember(dest => dest.ContactTitle, opt => opt.MapFrom(src => src.ContactTitle))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Region))
            .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.Fax, opt => opt.MapFrom(src => src.Fax))
            .ForMember(dest => dest.HomePage, opt => opt.MapFrom(src => src.HomePage));
    }
}
