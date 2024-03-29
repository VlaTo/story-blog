using AutoMapper;
using StoryBlog.Web.Microservices.Communication.Application.Handlers.NewPostCreated;
using StoryBlog.Web.Microservices.Communication.Application.Models;

namespace StoryBlog.Web.Microservices.Communication.Application.Extensions;

public static class AutoMapperConfigurationExpressionExtensions
{
    public static IMapperConfigurationExpression AddApplicationMappingProfiles(this IMapperConfigurationExpression mapper)
    {
        mapper.CreateMap<NewPostCreatedCommand, PublishNewPostCreated>()
            .ForMember(destination => destination.PostKey, source => source.MapFrom(x => x.PostKey))
            .ForMember(destination => destination.Slug, source => source.MapFrom(x => x.Slug))
            .ForMember(destination => destination.AuthorId, source => source.MapFrom(x => x.AuthorId))
            .ForMember(destination => destination.Created, source => source.MapFrom(x => x.Created))
            ;

        return mapper;
    }
}