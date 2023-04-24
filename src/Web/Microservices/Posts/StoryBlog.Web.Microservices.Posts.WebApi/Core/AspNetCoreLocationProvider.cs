using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Microservices.Posts.WebApi.Configuration;

namespace StoryBlog.Web.Microservices.Posts.WebApi.Core;

internal sealed class AspNetCoreLocationProvider : ILocationProvider
{
    private readonly IUrlHelperFactory urlHelperFactory;
    private readonly PostLocationProviderOptions options;

    public AspNetCoreLocationProvider(IUrlHelperFactory urlHelperFactory, IOptions<PostLocationProviderOptions> options)
    {
        this.urlHelperFactory = urlHelperFactory;
        this.options = options.Value;
    }

    public Uri? GetPostUri(ControllerContext controllerContext, string routeName, Guid postKey)
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