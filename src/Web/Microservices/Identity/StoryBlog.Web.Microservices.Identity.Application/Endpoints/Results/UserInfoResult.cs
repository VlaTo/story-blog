using Microsoft.AspNetCore.Http;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Hosting;

namespace StoryBlog.Web.Microservices.Identity.Application.Endpoints.Results;

internal sealed class UserInfoResult : IEndpointResult
{
    public Dictionary<string, object> Claims
    {
        get;
    }

    public UserInfoResult(Dictionary<string, object> claims)
    {
        Claims = claims;
    }

    public Task ExecuteAsync(HttpContext context)
    {
        context.Response.SetNoCache();
        return context.Response.WriteJsonAsync(Claims);
    }
}