﻿using AutoMapper;
using StoryBlog.Web.Microservices.Posts.Application.Models;

namespace StoryBlog.Web.Microservices.Posts.Application.Extensions;

public static class AutoMapperConfigurationExpressionExtensions
{
    public static IMapperConfigurationExpression AddApplicationMappingProfiles(this IMapperConfigurationExpression expression)
    {
        expression.CreateMap<Domain.Entities.Post, Post>()
            .ForMember(destination => destination.Key, source => source.MapFrom(x => x.Key))
            .ForMember(destination => destination.Title, source => source.MapFrom(x => x.Title))
            .ForMember(destination => destination.Slug, source => source.MapFrom(x => x.Slug.Text))
            .ForMember(destination => destination.Status, source => source.MapFrom(x => x.Status))
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