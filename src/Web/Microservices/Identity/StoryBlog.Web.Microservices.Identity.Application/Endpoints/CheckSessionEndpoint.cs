using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Hosting;
using System.Net;
using StoryBlog.Web.Microservices.Identity.Application.Endpoints.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.Endpoints;

internal sealed class CheckSessionEndpoint : IEndpointHandler
{
    private readonly ILogger logger;

    public CheckSessionEndpoint(ILogger<CheckSessionEndpoint> logger)
    {
        this.logger = logger;
    }

    public Task<IEndpointResult?> ProcessAsync(HttpContext context)
    {
        using var activity = Tracing.BasicActivitySource.StartActivity(Constants.EndpointNames.CheckSession + "Endpoint");

        IEndpointResult result;

        if (false == HttpMethods.IsGet(context.Request.Method))
        {
            logger.LogWarning("Invalid HTTP method for check session endpoint");
            result = new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
        }
        else
        {
            logger.LogDebug("Rendering check session result");
            result = new CheckSessionResult();
        }

        return Task.FromResult<IEndpointResult?>(result);
    }
}