using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Microservices.Comments.WebApi.Configuration;

namespace StoryBlog.Web.Microservices.Comments.WebApi.Core;

internal sealed class AspNetCoreLocationProvider : ILocationProvider
{
    private readonly IUrlHelperFactory urlHelperFactory;
    private readonly CommentLocationProviderOptions options;

    public AspNetCoreLocationProvider(IUrlHelperFactory urlHelperFactory, IOptions<CommentLocationProviderOptions> options)
    {
        this.urlHelperFactory = urlHelperFactory;
        this.options = options.Value;
    }

    public Uri? GetCommentUri(ControllerContext controllerContext, string routeName, Guid postKey)
    {
        if (options.UseUrlHelper)
        {
            var urlHelper = urlHelperFactory.GetUrlHelper(controllerContext);
            var location = urlHelper.RouteUrl(routeName, new { key = postKey });

            return null != location ? new Uri(location, UriKind.Relative) : null;
        }

        return null != options.ExternalUrlTemplate
            ? new Uri(String.Format(options.ExternalUrlTemplate, postKey))
            : null;
    }
}