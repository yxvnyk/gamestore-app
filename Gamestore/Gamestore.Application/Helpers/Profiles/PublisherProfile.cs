using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Models.DTO.Publisher;

namespace Gamestore.Application.Helpers.Profiles;

public class PublisherProfile : Profile
{
    public PublisherProfile()
    {
        CreateMap<PublisherDto, Publisher>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Games, opt => opt.Ignore());
    }
}
