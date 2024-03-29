using AutoMapper;
using StoryBlog.Web.Microservices.Communication.Application.Handlers.NewPostCreated;
using StoryBlog.Web.Microservices.Communication.Application.Models;
using StoryBlog.Web.Microservices.Communication.MessageHub.Messages;
using StoryBlog.Web.Microservices.Posts.Events;

namespace StoryBlog.Web.Microservices.Communication.WebApi.Extensions;

internal static class AutoMapperConfigurationExpressionExtensions
{
    public static IMapperConfigurationExpression AddWebApiMappingProfiles(this IMapperConfigurationExpression mapper)
    {
        mapper.CreateMap<NewPostCreatedEvent, NewPostCreatedCommand>()
            .ForMember(destination => destination.PostKey, source => source.MapFrom(x => x.PostKey))
            .ForMember(destination => destination.Slug, source => source.MapFrom(x => x.Slug))
            .ForMember(destination => destination.AuthorId, source => source.MapFrom(x => x.AuthorId))
            .ForMember(destination => destination.Created, source => source.MapFrom(x => x.Created))
            ;

        mapper.CreateMap<PublishNewPostCreated, NewPostPublishedHubMessage>()
            .ForMember(destination => destination.PostKey, source => source.MapFrom(x => x.PostKey))
            .ForMember(destination => destination.Slug, source => source.MapFrom(x => x.Slug))
            .ForMember(destination => destination.AuthorId, source => source.MapFrom(x => x.AuthorId))
            .ForMember(destination => destination.CreatedAt, source => source.MapFrom(x => x.Created))
            ;

        return mapper;
    }
}