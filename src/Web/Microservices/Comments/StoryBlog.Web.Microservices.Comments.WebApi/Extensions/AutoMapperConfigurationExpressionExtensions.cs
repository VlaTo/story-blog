using AutoMapper;
using StoryBlog.Web.Microservices.Comments.Application.Handlers.NewPostCreated;
using StoryBlog.Web.Microservices.Comments.Application.Models;
using StoryBlog.Web.Microservices.Comments.Events;
using StoryBlog.Web.Microservices.Comments.Shared.Models;
using StoryBlog.Web.Microservices.Posts.Events;

namespace StoryBlog.Web.Microservices.Comments.WebApi.Extensions;

public static class AutoMapperConfigurationExpressionExtensions
{
    public static IMapperConfigurationExpression AddWebApiMappingProfiles(this IMapperConfigurationExpression expression)
    {
        expression.CreateMap<Comment, CommentModel>()
            .ForMember(destination => destination.Key, source => source.MapFrom(x => x.Key))
            .ForMember(destination => destination.Text, source => source.MapFrom(x => x.Text))
            .ForMember(destination => destination.Comments, source => source.MapFrom(x => x.Comments))
            .ForMember(destination => destination.PublicationStatus, source => source.MapFrom(x => x.PublicationStatus))
            .ForMember(destination => destination.AuthorId, source => source.MapFrom(x => x.Author))
            .ForMember(destination => destination.CreatedAt, source => source.MapFrom(x => x.CreatedAt))
            ;

        expression.CreateMap<Comment, CreatedCommentModel>()
            .ForMember(destination => destination.Key, source => source.MapFrom(x => x.Key))
            .ForMember(destination => destination.PostKey, source => source.MapFrom(x => x.PostKey))
            .ForMember(destination => destination.PostKey, source => source.MapFrom(x => x.ParentKey))
            .ForMember(destination => destination.Text, source => source.MapFrom(x => x.Text))
            .ForMember(destination => destination.AuthorId, source => source.MapFrom(x => x.Author))
            .ForMember(destination => destination.PublicationStatus, source => source.MapFrom(x => x.PublicationStatus))
            .ForMember(destination => destination.CreatedAt, source => source.MapFrom(x => x.CreatedAt))
            ;

        expression.CreateMap<NewCommentCreated, NewCommentCreatedEvent>()
            ;

        expression.CreateMap<NewPostCreatedEvent, NewPostCreatedCommand>()
            .ForMember(destination => destination.PostKey, source => source.MapFrom(x => x.PostKey))
            ;

        return expression;
    }
}