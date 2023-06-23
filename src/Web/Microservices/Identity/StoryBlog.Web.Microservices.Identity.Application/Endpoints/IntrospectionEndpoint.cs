using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Core.Events;
using StoryBlog.Web.Microservices.Identity.Application.Endpoints.Results;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Hosting;
using StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Generators;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Application.Validation;
using System.Net;

namespace StoryBlog.Web.Microservices.Identity.Application.Endpoints;

/// <summary>
/// Introspection endpoint
/// </summary>
/// <seealso cref="IEndpointHandler" />
internal sealed class IntrospectionEndpoint : IEndpointHandler
{
    private readonly IIntrospectionResponseGenerator responseGenerator;
    private readonly IEventService events;
    private readonly IIntrospectionRequestValidator requestValidator;
    private readonly IApiSecretValidator apiSecretValidator;
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntrospectionEndpoint" /> class.
    /// </summary>
    /// <param name="apiSecretValidator">The API secret validator.</param>
    /// <param name="requestValidator">The request validator.</param>
    /// <param name="responseGenerator">The generator.</param>
    /// <param name="events">The events.</param>
    /// <param name="logger">The logger.</param>
    public IntrospectionEndpoint(
        IApiSecretValidator apiSecretValidator,
        IIntrospectionRequestValidator requestValidator,
        IIntrospectionResponseGenerator responseGenerator,
        IEventService events,
        ILogger<IntrospectionEndpoint> logger)
    {
        this.apiSecretValidator = apiSecretValidator;
        this.requestValidator = requestValidator;
        this.responseGenerator = responseGenerator;
        this.events = events;
        this.logger = logger;
    }

    /// <summary>
    /// Processes the request.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns></returns>
    public async Task<IEndpointResult?> ProcessAsync(HttpContext context)
    {
        using var activity = Tracing.BasicActivitySource.StartActivity(Constants.EndpointNames.Introspection + "Endpoint");

        logger.LogTrace("Processing introspection request.");

        // validate HTTP
        if (false == HttpMethods.IsPost(context.Request.Method))
        {
            logger.LogWarning("Introspection endpoint only supports POST requests");

            return new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
        }

        if (false == context.Request.HasApplicationFormContentType())
        {
            logger.LogWarning("Invalid media type for introspection endpoint");

            return new StatusCodeResult(HttpStatusCode.UnsupportedMediaType);
        }

        try
        {
            return await ProcessIntrospectionRequestAsync(context);
        }
        catch (InvalidDataException ex)
        {
            logger.LogWarning(ex, "Invalid HTTP request for introspection endpoint");

            return new StatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
    
    private async Task<IEndpointResult> ProcessIntrospectionRequestAsync(HttpContext context)
    {
        logger.LogDebug("Starting introspection request.");

        // caller validation
        var apiResult = await apiSecretValidator.ValidateAsync(context);

        if (null == apiResult.Resource)
        {
            logger.LogError("API unauthorized to call introspection endpoint. aborting.");
            return new StatusCodeResult(HttpStatusCode.Unauthorized);
        }

        var body = await context.Request.ReadFormAsync();

        if (null == body)
        {
            logger.LogError("Malformed request body. aborting.");
            
            await events.RaiseAsync(new TokenIntrospectionFailureEvent(apiResult.Resource.Name, "Malformed request body"));

            return new StatusCodeResult(HttpStatusCode.BadRequest);
        }

        // request validation
        logger.LogTrace("Calling into introspection request validator: {type}", requestValidator.GetType().FullName);

        var validationResult = await requestValidator.ValidateAsync(body.AsNameValueCollection(), apiResult.Resource);

        if (validationResult.IsError)
        {
            LogFailure(validationResult.Error, apiResult.Resource.Name);

            await events.RaiseAsync(new TokenIntrospectionFailureEvent(apiResult.Resource.Name, validationResult.Error));

            return new BadRequestResult(validationResult.Error);
        }

        // response generation
        logger.LogTrace("Calling into introspection response generator: {type}", responseGenerator.GetType().FullName);

        var response = await responseGenerator.ProcessAsync(validationResult);

        // render result
        LogSuccess(validationResult.IsActive, validationResult.Api.Name);

        return new IntrospectionResult(response);
    }
    private void LogSuccess(bool tokenActive, string apiName)
    {
        logger.LogInformation("Success token introspection. Token active: {tokenActive}, for API name: {apiName}", tokenActive, apiName);
    }

    private void LogFailure(string error, string apiName)
    {
        logger.LogError("Failed token introspection: {error}, for API name: {apiName}", error, apiName);
    }
}