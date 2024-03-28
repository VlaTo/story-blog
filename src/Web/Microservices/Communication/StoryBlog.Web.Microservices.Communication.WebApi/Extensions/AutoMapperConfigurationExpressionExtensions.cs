using AutoMapper;

namespace StoryBlog.Web.Microservices.Communication.WebApi.Extensions;

internal static class AutoMapperConfigurationExpressionExtensions
{
    public static IMapperConfigurationExpression AddWebApiMappingProfiles(this IMapperConfigurationExpression mapper)
    {

        return mapper;
    }
}