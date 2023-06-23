using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Core.Events;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Application.Stores;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation;

/// <summary>
/// Validates API secrets using the registered secret validators and parsers
/// </summary>
public sealed class ApiSecretValidator : IApiSecretValidator
{
    private readonly IResourceStore resources;
    private readonly ISecretsListParser parser;
    private readonly ISecretsListValidator validator;
    private readonly IEventService events;
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiSecretValidator"/> class.
    /// </summary>
    /// <param name="resources">The resources.</param>
    /// <param name="parser">The parsers.</param>
    /// <param name="validator">The validator.</param>
    /// <param name="events">The events.</param>
    /// <param name="logger">The logger.</param>
    public ApiSecretValidator(
        IResourceStore resources,
        ISecretsListParser parser,
        ISecretsListValidator validator,
        IEventService events,
        ILogger<ApiSecretValidator> logger)
    {
        this.resources = resources;
        this.parser = parser;
        this.validator = validator;
        this.events = events;
        this.logger = logger;
    }

    /// <summary>
    /// Validates the secret on the current request.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public async Task<ApiSecretValidationResult> ValidateAsync(HttpContext context)
    {
        using var activity = Tracing.ValidationActivitySource.StartActivity("ApiSecretValidator.Validate");

        logger.LogTrace("Start API validation");

        var fail = new ApiSecretValidationResult
        {
            IsError = true
        };

        var parsedSecret = await parser.ParseAsync(context);

        if (null == parsedSecret)
        {
            await RaiseFailureEventAsync("unknown", "No API id or secret found");

            logger.LogError("No API secret found");

            return fail;
        }

        // load API resource
        var apis = await resources.FindApiResourcesByNameAsync(new[] { parsedSecret.Id });

        if (null == apis || false == apis.Any())
        {
            await RaiseFailureEventAsync(parsedSecret.Id, "Unknown API resource");

            logger.LogError("No API resource with that name found. aborting");

            return fail;
        }

        if (1 < apis.Count())
        {
            await RaiseFailureEventAsync(parsedSecret.Id, "Invalid API resource");

            logger.LogError("More than one API resource with that name found. aborting");

            return fail;
        }

        var api = apis.Single();

        if (false == api.Enabled)
        {
            await RaiseFailureEventAsync(parsedSecret.Id, "API resource not enabled");

            logger.LogError("API resource not enabled. aborting.");

            return fail;
        }

        var result = await validator.ValidateAsync(api.ApiSecrets, parsedSecret);

        if (result.Success)
        {
            logger.LogDebug("API resource validation success");

            var success = new ApiSecretValidationResult
            {
                IsError = false,
                Resource = api
            };

            await RaiseSuccessEventAsync(api.Name, parsedSecret.Type);

            return success;
        }

        await RaiseFailureEventAsync(api.Name, "Invalid API secret");
        
        logger.LogError("API validation failed.");

        return fail;
    }

    private Task RaiseSuccessEventAsync(string clientId, string authMethod)
    {
        return events.RaiseAsync(new ApiAuthenticationSuccessEvent(clientId, authMethod));
    }

    private Task RaiseFailureEventAsync(string clientId, string message)
    {
        return events.RaiseAsync(new ApiAuthenticationFailureEvent(clientId, message));
    }
}