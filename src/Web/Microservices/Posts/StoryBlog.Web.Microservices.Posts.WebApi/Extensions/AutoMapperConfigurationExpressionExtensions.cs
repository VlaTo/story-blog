using AutoMapper;
using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Microservices.Posts.WebApi.Extensions;

internal static class AutoMapperConfigurationExpressionExtensions
{
    public static IMapperConfigurationExpression AddWebApiMappingProfiles(this IMapperConfigurationExpression expression)
    {
        /*expression.CreateMap<Application.Models.AllowedActions, AllowedActions>()
            .ForMember(destination => destination.CanEdit, source => source.MapFrom(x => x.CanEdit))
            .ForMember(destination => destination.CanDelete, source => source.MapFrom(x => x.CanDelete))
            ;*/

        expression.CreateMap<Application.Models.Brief, BriefModel>()
            .ForMember(destination => destination.Key, source => source.MapFrom(x => x.Key))
            .ForMember(destination => destination.Title, source => source.MapFrom(x => x.Title))
            .ForMember(destination => destination.Slug, source => source.MapFrom(x => x.Slug))
            .ForMember(destination => destination.Text, source => source.MapFrom(x => x.Text))
            .ForMember(destination => destination.Author, source => source.MapFrom(x => x.Author))
            .ForMember(destination => destination.PublicationStatus, source => source.MapFrom(x => x.PublicationStatus))
            .ForMember(destination => destination.AllowedActions, source => source.MapFrom(x => x.AllowedActions))
            .ForMember(destination => destination.CommentsCount, source => source.MapFrom(x => x.CommentsCount))
            .ForMember(destination => destination.VisibilityStatus, source => source.MapFrom(x => x.VisibilityStatus))
            .ForMember(destination => destination.CreatedAt, source => source.MapFrom(x => x.CreatedAt))
            ;

        expression.CreateMap<Application.Models.Post, PostModel>()
            .ForMember(destination => destination.Key, source => source.MapFrom(x => x.Key))
            .ForMember(destination => destination.Title, source => source.MapFrom(x => x.Title))
            .ForMember(destination => destination.Slug, source => source.MapFrom(x => x.Slug))
            .ForMember(destination => destination.Text, source => source.MapFrom(x => x.Text))
            .ForMember(destination => destination.PublicationStatus, source => source.MapFrom(x => x.PublicationStatus))
            .ForMember(destination => destination.CommentsCount, source => source.MapFrom(x => x.CommentsCount))
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
            .ForMember(destination => destination.PublicationStatus, source => source.MapFrom(x => x.PublicationStatus))
            .ForMember(destination => destination.CreatedAt, source => source.MapFrom(x => x.CreatedAt))
            ;

        expression.CreateMap<Application.Models.PostReference, PostReferenceModel>()
            .ForMember(destination => destination.Key, source => source.MapFrom(x => x.Key))
            .ForMember(destination => destination.Title, source => source.MapFrom(x => x.Title))
            .ForMember(destination => destination.Slug, source => source.MapFrom(x => x.Slug))
            .ForMember(destination => destination.Status, source => source.MapFrom(x => x.PublicationStatus))
            ;

        return expression;
    }
}