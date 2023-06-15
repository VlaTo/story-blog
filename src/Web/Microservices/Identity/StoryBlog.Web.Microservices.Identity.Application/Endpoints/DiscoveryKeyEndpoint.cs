using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Endpoints.Results;
using StoryBlog.Web.Microservices.Identity.Application.Hosting;
using StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Generators;

namespace StoryBlog.Web.Microservices.Identity.Application.Endpoints;

internal sealed class DiscoveryKeyEndpoint : IEndpointHandler
{
    private readonly IdentityServerOptions options;
    private readonly IDiscoveryResponseGenerator responseGenerator;
    private readonly ILogger logger;

    public DiscoveryKeyEndpoint(
        IdentityServerOptions options,
        IDiscoveryResponseGenerator responseGenerator,
        ILogger<DiscoveryKeyEndpoint> logger)
    {
        this.options = options;
        this.responseGenerator = responseGenerator;
        this.logger = logger;
    }

    public async Task<IEndpointResult?> ProcessAsync(HttpContext context)
    {
        using var activity = Tracing.BasicActivitySource.StartActivity(Constants.EndpointNames.Discovery + "Endpoint");

        logger.LogTrace("Processing discovery request.");

        // validate HTTP
        if (!HttpMethods.IsGet(context.Request.Method))
        {
            logger.LogWarning("Discovery endpoint only supports GET requests");
            return new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
        }

        logger.LogDebug("Start key discovery request");

        if (false == options.Discovery.ShowKeySet)
        {
            logger.LogInformation("Key discovery disabled. 404.");
            return new StatusCodeResult(HttpStatusCode.NotFound);
        }

        // generate response
        logger.LogTrace("Calling into discovery response generator: {type}", responseGenerator.GetType().FullName);
        
        var response = await responseGenerator.CreateJwkDocumentAsync();

        return new JsonWebKeysResult(response, options.Discovery.ResponseCacheInterval);
    }
}