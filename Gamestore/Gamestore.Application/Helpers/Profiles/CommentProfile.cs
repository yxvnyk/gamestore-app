using AutoMapper;
using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Models.DTO.Comments;

namespace Gamestore.Application.Helpers.Profiles;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<CommentCreateDto, Comment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Comment.Name))
            .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Comment.Body))
            .ForMember(dest => dest.ParentCommentId, opt => opt.MapFrom(src => src.ParentId));

        CreateMap<Comment, CommentTreeDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body))
            .ForMember(dest => dest.ChildCommnets, opt => opt.MapFrom(src => src.ChildComments));
    }
}
