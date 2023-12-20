using AutoMapper;
using StoryBlog.Web.Microservices.Comments.Application.Models;

namespace StoryBlog.Web.Microservices.Comments.Application.Extensions;

public static class AutoMapperConfigurationExpressionExtensions
{
    public static IMapperConfigurationExpression AddApplicationMappingProfiles(this IMapperConfigurationExpression expression)
    {
        expression.CreateMap<Domain.Entities.Comment, Comment>()
            .ForMember(destination => destination.Key, source => source.MapFrom(x => x.Key))
            .ForMember(destination => destination.PostKey, source => source.MapFrom(x => x.PostKey))
            .ForMember(destination => destination.ParentKey, source => source.MapFrom(x => x.Parent.Key))
            .ForMember(destination => destination.Text, source => source.MapFrom(x => x.Text))
            .ForMember(destination => destination.Author, source => source.MapFrom(x => x.AuthorId))
            .ForMember(destination => destination.Comments, source => source.MapFrom(x => x.Comments))
            .ForMember(destination => destination.PublicationStatus, source => source.MapFrom(x => x.PublicationStatus))
            .ForMember(destination => destination.CreatedAt, source => source.MapFrom(x => x.CreateAt))
            ;

        return expression;
    }
}