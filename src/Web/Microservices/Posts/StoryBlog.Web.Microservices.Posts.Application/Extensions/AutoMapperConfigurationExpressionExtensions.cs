using AutoMapper;
using StoryBlog.Web.Microservices.Posts.Application.Models;

namespace StoryBlog.Web.Microservices.Posts.Application.Extensions;

public static class AutoMapperConfigurationExpressionExtensions
{
    public static IMapperConfigurationExpression AddApplicationMappingProfiles(this IMapperConfigurationExpression expression)
    {
        /*expression.CreateMap<Domain.Entities.Post, Brief>()
            .ForMember(destination => destination.Key, source => source.MapFrom(x => x.Key))
            .ForMember(destination => destination.Title, source => source.MapFrom(x => x.Title))
            .ForMember(destination => destination.Slug, source => source.MapFrom(x => x.Slug.Text))
            .ForMember(destination => destination.Status, source => source.MapFrom(x => x.Status))
            .ForMember(destination => destination.Text, source => source.MapFrom(x => x.Content.Brief))
            .ForMember(destination => destination.CommentsCount, source => source.MapFrom(x => x.CommentsCounter.Counter))
            .ForMember(destination => destination.CreatedAt, source => source.MapFrom(x => x.CreateAt))
            ;*/

        expression.CreateMap<Domain.Entities.Post, Post>()
            .ForMember(destination => destination.Key, source => source.MapFrom(x => x.Key))
            .ForMember(destination => destination.Title, source => source.MapFrom(x => x.Title))
            .ForMember(destination => destination.Slug, source => source.MapFrom(x => x.Slug.Text))
            .ForMember(destination => destination.Status, source => source.MapFrom(x => x.Status))
            .ForMember(destination => destination.Text, source => source.MapFrom(x => x.Content.Text))
            .ForMember(destination => destination.CommentsCount, source => source.MapFrom(x => x.CommentsCounter.Counter))
            .ForMember(destination => destination.CreatedAt, source => source.MapFrom(x => x.CreateAt))
            ;

        expression.CreateMap<Domain.Entities.Post, PostReference>()
            .ForMember(destination => destination.Key, source => source.MapFrom(x => x.Key))
            .ForMember(destination => destination.Title, source => source.MapFrom(x => x.Title))
            .ForMember(destination => destination.Slug, source => source.MapFrom(x => x.Slug.Text))
            .ForMember(destination => destination.Status, source => source.MapFrom(x => x.Status))
            ;

        return expression;
    }
}