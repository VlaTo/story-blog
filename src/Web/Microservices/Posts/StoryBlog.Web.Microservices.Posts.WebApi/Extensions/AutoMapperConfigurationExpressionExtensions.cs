using AutoMapper;
using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Microservices.Posts.WebApi.Extensions;

internal static class AutoMapperConfigurationExpressionExtensions
{
    public static IMapperConfigurationExpression AddWebApiMappingProfiles(this IMapperConfigurationExpression expression)
    {
        expression.CreateMap<Application.Models.Post, PostModel>()
            .ForMember(destination => destination.Key, source => source.MapFrom(x => x.Key))
            .ForMember(destination => destination.Title, source => source.MapFrom(x => x.Title))
            .ForMember(destination => destination.Slug, source => source.MapFrom(x => x.Slug))
            .ForMember(destination => destination.Text, source => source.MapFrom(x => x.Text))
            .ForMember(destination => destination.Status, source => source.MapFrom(x => x.Status))
            .ForMember(destination => destination.CreatedAt, source => source.MapFrom(x => x.CreatedAt))
            ;

        expression.CreateMap<Application.Models.PostDetails, CreatedPostModel>()
            .ForMember(destination => destination.Title, source => source.MapFrom(x => x.Title))
            .ForMember(destination => destination.Slug, source => source.MapFrom(x => x.Slug))
            ;

        expression.CreateMap<Application.Models.Post, CreatedPostModel>()
            .ForMember(destination => destination.Key, source => source.MapFrom(x => x.Key))
            .ForMember(destination => destination.Title, source => source.MapFrom(x => x.Title))
            .ForMember(destination => destination.Slug, source => source.MapFrom(x => x.Slug))
            .ForMember(destination => destination.Status, source => source.MapFrom(x => x.Status))
            .ForMember(destination => destination.CreatedAt, source => source.MapFrom(x => x.CreatedAt))
            ;

        expression.CreateMap<Application.Models.PostReference, PostReferenceModel>()
            .ForMember(destination => destination.Key, source => source.MapFrom(x => x.Key))
            .ForMember(destination => destination.Title, source => source.MapFrom(x => x.Title))
            .ForMember(destination => destination.Slug, source => source.MapFrom(x => x.Slug))
            .ForMember(destination => destination.Status, source => source.MapFrom(x => x.Status))
            ;

        return expression;
    }
}