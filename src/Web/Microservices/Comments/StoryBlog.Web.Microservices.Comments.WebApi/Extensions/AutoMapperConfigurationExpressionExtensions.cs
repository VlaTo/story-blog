using AutoMapper;
using StoryBlog.Web.Microservices.Comments.Shared.Models;

namespace StoryBlog.Web.Microservices.Comments.WebApi.Extensions;

public static class AutoMapperConfigurationExpressionExtensions
{
    public static IMapperConfigurationExpression AddWebApiMappingProfiles(this IMapperConfigurationExpression expression)
    {
        expression.CreateMap<Application.Models.Comment, CommentModel>()
            .ForMember(destination => destination.Key, source => source.MapFrom(x => x.Key))
            .ForMember(destination => destination.Text, source => source.MapFrom(x => x.Text))
            //.ForMember(destination => destination.Slug, source => source.MapFrom(x => x.Slug))
            //.ForMember(destination => destination.Status, source => source.MapFrom(x => x.Status))
            //.ForMember(destination => destination.CreatedAt, source => source.MapFrom(x => x.CreatedAt))
            ;

        return expression;
    }
}