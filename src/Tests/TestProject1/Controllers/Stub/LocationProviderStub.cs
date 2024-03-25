using Microsoft.AspNetCore.Mvc;
using StoryBlog.Web.Microservices.Comments.WebApi.Core;

namespace TestProject1.Controllers.Stub;

internal sealed class LocationProviderStub : ILocationProvider
{
    public Uri? GetCommentUri(ControllerContext controllerContext, string routeName, Guid postKey)
    {
        throw new NotImplementedException();
    }
}