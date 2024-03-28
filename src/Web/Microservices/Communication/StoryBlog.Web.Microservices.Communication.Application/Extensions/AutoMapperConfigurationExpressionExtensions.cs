using AutoMapper;

namespace StoryBlog.Web.Microservices.Communication.Application.Extensions;

public static class AutoMapperConfigurationExpressionExtensions
{
    public static IMapperConfigurationExpression AddApplicationMappingProfiles(this IMapperConfigurationExpression mapper)
    {

        return mapper;
    }
}