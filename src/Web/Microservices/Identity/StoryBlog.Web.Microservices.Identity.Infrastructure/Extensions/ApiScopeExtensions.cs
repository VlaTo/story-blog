using StoryBlog.Web.Microservices.Identity.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Extensions;

internal static class ApiScopeExtensions
{
    public static void UseProperties(this Application.Storage.ApiScope scope, IList<ApiScopeProperty> properties)
    {
        ;
    }

    public static void UseClaims(this Application.Storage.ApiScope scope, IList<ApiScopeClaim> claims)
    {
        ;
    }
}