using Microsoft.AspNetCore.Http;

namespace StoryBlog.Web.Microservices.Identity.Application.Core;

public interface IAbsoluteUrlFactory
{
    string GetAbsoluteUrl(string path);

    string GetAbsoluteUrl(HttpContext context, string path);
}