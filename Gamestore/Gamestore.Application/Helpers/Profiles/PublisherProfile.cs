using AutoMapper;
using Gamestore.DataAccess.Entities;
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
    }
}
