using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Core.Events;
using StoryBlog.Web.Microservices.Identity.Application.Endpoints.Results;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Hosting;
using StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Generators;
using StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Models;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Application.Validation;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.Endpoints;

internal sealed class BackchannelAuthenticationEndpoint : IEndpointHandler
{
    private readonly IClientSecretValidator clientValidator;
    private readonly IBackchannelAuthenticationRequestValidator requestValidator;
    private readonly IBackchannelAuthenticationResponseGenerator responseGenerator;
    private readonly IEventService events;
    private readonly IdentityServerOptions options;
    private readonly ILogger<BackchannelAuthenticationEndpoint> logger;

    public BackchannelAuthenticationEndpoint(
        IClientSecretValidator clientValidator,
        IBackchannelAuthenticationRequestValidator requestValidator,
        IBackchannelAuthenticationResponseGenerator responseGenerator,
        IEventService events,
        IdentityServerOptions options,
        ILogger<BackchannelAuthenticationEndpoint> logger)
    {
        this.clientValidator = clientValidator;
        this.requestValidator = requestValidator;
        this.responseGenerator = responseGenerator;
        this.events = events;
        this.options = options;
        this.logger = logger;
    }

    public async Task<IEndpointResult?> ProcessAsync(HttpContext context)
    {
        using var activity = Tracing.BasicActivitySource.StartActivity(Constants.EndpointNames.BackchannelAuthentication + "Endpoint");

        logger.LogTrace("Processing backchannel authentication request.");

        // validate HTTP
        if (!HttpMethods.IsPost(context.Request.Method) || !context.Request.HasApplicationFormContentType())
        {
            logger.LogWarning("Invalid HTTP request for backchannel authentication endpoint");
            return Error(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidRequest);
        }

        try
        {
            return await ProcessAuthenticationRequestAsync(context);
        }
        catch (InvalidDataException ex)
        {
            logger.LogWarning(ex, "Invalid HTTP request for backchannel authentication endpoint");
            return Error(OidcConstants.BackchannelAuthenticationRequestErrors.InvalidRequest);
        }
    }

    private async Task<IEndpointResult> ProcessAuthenticationRequestAsync(HttpContext context)
    {
        logger.LogDebug("Start backchannel authentication request.");

        // validate client
        var clientResult = await clientValidator.ValidateAsync(context);
        if (clientResult.IsError)
        {
            return Error(clientResult.Error ?? OidcConstants.BackchannelAuthenticationRequestErrors.InvalidClient);
        }

        // validate request
        var form = (await context.Request.ReadFormAsync()).AsNameValueCollection();
        
        logger.LogTrace("Calling into backchannel authentication request validator: {type}", requestValidator.GetType().FullName);
        
        var requestResult = await requestValidator.ValidateRequestAsync(form, clientResult);

        if (requestResult.IsError)
        {
            await events.RaiseAsync(new BackchannelAuthenticationFailureEvent(requestResult));
            return Error(requestResult.Error, requestResult.ErrorDescription);
        }

        // create response
        logger.LogTrace("Calling into backchannel authentication request response generator: {type}", responseGenerator.GetType().FullName);
        
        var response = await responseGenerator.ProcessAsync(requestResult);

        await events.RaiseAsync(new BackchannelAuthenticationSuccessEvent(requestResult));

        LogResponse(response, requestResult);

        // return result
        logger.LogDebug("Backchannel authentication request success.");
        
        return new BackchannelAuthenticationResult(response);
    }

    private void LogResponse(BackchannelAuthenticationResponse response, BackchannelAuthenticationRequestValidationResult requestResult)
    {
        logger.LogTrace("BackchannelAuthenticationResponse: {@response} for subject {subjectId}", response, requestResult.ValidatedRequest.Subject.GetSubjectId());
    }

    BackchannelAuthenticationResult Error(string? error, string? errorDescription = null)
    {
        return new BackchannelAuthenticationResult(new BackchannelAuthenticationResponse(error, errorDescription));
    }
}