using AutoMapper;
using StoryBlog.Web.Microservices.Comments.Events;
using StoryBlog.Web.Microservices.Posts.Application.Handlers.NewCommentCreated;
using StoryBlog.Web.Microservices.Posts.Application.Models;
using StoryBlog.Web.Microservices.Posts.Events;
using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Microservices.Posts.WebApi.Extensions;

internal static class AutoMapperConfigurationExpressionExtensions
{
    public static IMapperConfigurationExpression AddWebApiMappingProfiles(this IMapperConfigurationExpression expression)
    {
        expression.CreateMap<Brief, BriefModel>()
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

        expression.CreateMap<Post, PostModel>()
            .ForMember(destination => destination.Key, source => source.MapFrom(x => x.Key))
            .ForMember(destination => destination.Title, source => source.MapFrom(x => x.Title))
            .ForMember(destination => destination.Slug, source => source.MapFrom(x => x.Slug))
            .ForMember(destination => destination.Text, source => source.MapFrom(x => x.Text))
            .ForMember(destination => destination.PublicationStatus, source => source.MapFrom(x => x.PublicationStatus))
            .ForMember(destination => destination.CommentsCount, source => source.MapFrom(x => x.CommentsCount))
            .ForMember(destination => destination.CreatedAt, source => source.MapFrom(x => x.CreatedAt))
            ;

        expression.CreateMap<PostDetails, CreatedPostModel>()
            .ForMember(destination => destination.Title, source => source.MapFrom(x => x.Title))
            .ForMember(destination => destination.Slug, source => source.MapFrom(x => x.Slug))
            ;

        expression.CreateMap<Post, CreatedPostModel>()
            .ForMember(destination => destination.Key, source => source.MapFrom(x => x.Key))
            .ForMember(destination => destination.Title, source => source.MapFrom(x => x.Title))
            .ForMember(destination => destination.Slug, source => source.MapFrom(x => x.Slug))
            .ForMember(destination => destination.PublicationStatus, source => source.MapFrom(x => x.PublicationStatus))
            .ForMember(destination => destination.CreatedAt, source => source.MapFrom(x => x.CreatedAt))
            ;

        expression.CreateMap<PostReference, PostReferenceModel>()
            .ForMember(destination => destination.Key, source => source.MapFrom(x => x.Key))
            .ForMember(destination => destination.Title, source => source.MapFrom(x => x.Title))
            .ForMember(destination => destination.Slug, source => source.MapFrom(x => x.Slug))
            .ForMember(destination => destination.Status, source => source.MapFrom(x => x.PublicationStatus))
            ;

        expression.CreateMap<NewPostCreated, NewPostCreatedEvent>()
            .ForMember(destination => destination.PostKey, source => source.MapFrom(x => x.Key))
            .ForMember(destination => destination.Created, source => source.MapFrom(x => x.CreatedAt))
            .ForMember(destination => destination.AuthorId, source => source.MapFrom(x => x.AuthorId))
            .ForMember(destination => destination.Slug, source => source.MapFrom(x => x.Slug))
            ;

        expression.CreateMap<NewCommentCreatedEvent, NewCommentCreatedCommand>()
            .ForMember(destination => destination.Key, source => source.MapFrom(x => x.Key))
            .ForMember(destination => destination.PostKey, source => source.MapFrom(x => x.PostKey))
            .ForMember(destination => destination.ParentKey, source => source.MapFrom(x => x.ParentKey))
            .ForMember(destination => destination.ApprovedCommentsCount, source => source.MapFrom(x => x.ApprovedCommentsCount))
            ;
        return expression;
    }
}