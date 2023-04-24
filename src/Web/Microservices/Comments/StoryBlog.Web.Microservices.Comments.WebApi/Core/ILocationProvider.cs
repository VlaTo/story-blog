using Microsoft.AspNetCore.Mvc;

namespace StoryBlog.Web.Microservices.Comments.WebApi.Core;

public interface ILocationProvider
{
    Uri? GetCommentUri(ControllerContext controllerContext, string routeName, Guid postKey);
}