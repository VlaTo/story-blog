using Microsoft.AspNetCore.Mvc;

namespace StoryBlog.Web.Microservices.Posts.WebApi.Core;

public interface ILocationProvider
{
    Uri? GetPostUri(ControllerContext controllerContext, string routeName, Guid postKey);
}