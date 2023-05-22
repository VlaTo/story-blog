using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Hosting;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using System.Net;
using StoryBlog.Web.Microservices.Identity.Application.Endpoints.Results;
using StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Generators;

namespace StoryBlog.Web.Microservices.Identity.Application.Endpoints;

internal sealed class DiscoveryEndpoint : IEndpointHandler
{
    private readonly IdentityServerOptions options;
    private readonly IIssuerNameService issuerNameService;
    private readonly IServerUrls urls;
    private readonly IDiscoveryResponseGenerator responseGenerator;
    private readonly ILogger logger;

    public DiscoveryEndpoint(
        IdentityServerOptions options,
        IIssuerNameService issuerNameService,
        IDiscoveryResponseGenerator responseGenerator,
        IServerUrls urls,
        ILogger<DiscoveryEndpoint> logger)
    {
        this.options = options;
        this.issuerNameService = issuerNameService;
        this.urls = urls;
        this.responseGenerator = responseGenerator;
        this.logger = logger;
    }

    public async Task<IEndpointResult?> ProcessAsync(HttpContext context)
    {
        using var activity = Tracing.ActivitySource.StartActivity(Constants.EndpointNames.Discovery + "Endpoint");

        logger.LogTrace("Processing discovery request.");

        // validate HTTP
        if (false == HttpMethods.IsGet(context.Request.Method))
        {
            logger.LogWarning("Discovery endpoint only supports GET requests");
            return new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
        }

        logger.LogDebug("Start discovery request");

        if (false == options.Endpoints.EnableDiscoveryEndpoint)
        {
            logger.LogInformation("Discovery endpoint disabled. 404.");
            return new StatusCodeResult(HttpStatusCode.NotFound);
        }

        var baseUrl = urls.BaseUrl;
        var issuerUri = await issuerNameService.GetCurrentAsync();

        // generate response
        logger.LogTrace("Calling into discovery response generator: {type}", responseGenerator.GetType().FullName);
        var response = await responseGenerator.CreateDiscoveryDocumentAsync(baseUrl, issuerUri);

        return new DiscoveryDocumentResult(response, options.Discovery.ResponseCacheInterval);
    }
}