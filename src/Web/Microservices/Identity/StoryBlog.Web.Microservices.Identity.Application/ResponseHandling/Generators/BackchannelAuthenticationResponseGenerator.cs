using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Models.Requests;
using StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Models;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Application.Stores;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Requests;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Generators;

/// <summary>
/// The backchannel authentication response generator
/// </summary>
/// <seealso cref="IBackchannelAuthenticationResponseGenerator" />
public class BackchannelAuthenticationResponseGenerator : IBackchannelAuthenticationResponseGenerator
{
    /// <summary>
    /// The options
    /// </summary>
    protected IdentityServerOptions Options
    {
        get;
    }

    /// <summary>
    /// The request store.
    /// </summary>
    protected IBackChannelAuthenticationRequestStore BackChannelAuthenticationRequestStore
    {
        get;
    }

    /// <summary>
    /// The user login service.
    /// </summary>
    protected IBackchannelAuthenticationUserNotificationService UserLoginService
    {
        get;
    }

    /// <summary>
    /// The clock
    /// </summary>
    protected ISystemClock Clock
    {
        get;
    }

    /// <summary>
    /// The logger
    /// </summary>
    protected ILogger Logger
    {
        get;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BackchannelAuthenticationResponseGenerator"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="backChannelAuthenticationRequestStore"></param>
    /// <param name="userLoginService"></param>
    /// <param name="clock">The clock.</param>
    /// <param name="logger">The logger.</param>
    public BackchannelAuthenticationResponseGenerator(IdentityServerOptions options,
        IBackChannelAuthenticationRequestStore backChannelAuthenticationRequestStore,
        IBackchannelAuthenticationUserNotificationService userLoginService,
        ISystemClock clock,
        ILogger<BackchannelAuthenticationResponseGenerator> logger)
    {
        Options = options;
        BackChannelAuthenticationRequestStore = backChannelAuthenticationRequestStore;
        UserLoginService = userLoginService;
        Clock = clock;
        Logger = logger;
    }

    /// <inheritdoc/>
    public virtual async Task<BackchannelAuthenticationResponse> ProcessAsync(BackchannelAuthenticationRequestValidationResult validationResult)
    {
        using var activity = Tracing.BasicActivitySource.StartActivity("BackchannelAuthenticationResponseGenerator.Process");

        if (null == validationResult)
        {
            throw new ArgumentNullException(nameof(validationResult));
        }

        if (null == validationResult.ValidatedRequest)
        {
            throw new ArgumentNullException(nameof(validationResult.ValidatedRequest));
        }

        if (null == validationResult.ValidatedRequest.Client)
        {
            throw new ArgumentNullException(nameof(validationResult.ValidatedRequest.Client));
        }

        Logger.LogTrace("Creating response for backchannel authentication request");

        var request = new BackChannelAuthenticationRequest
        {
            CreationTime = Clock.UtcNow.UtcDateTime,
            ClientId = validationResult.ValidatedRequest.ClientId,
            RequestedScopes = validationResult.ValidatedRequest.ValidatedResources.RawScopeValues,
            RequestedResourceIndicators = validationResult.ValidatedRequest.RequestedResourceIndicators,
            Subject = validationResult.ValidatedRequest.Subject,
            Lifetime = validationResult.ValidatedRequest.Expiry,
            AuthenticationContextReferenceClasses = validationResult.ValidatedRequest.AuthenticationContextReferenceClasses,
            Tenant = validationResult.ValidatedRequest.Tenant,
            IdP = validationResult.ValidatedRequest.IdP,
            BindingMessage = validationResult.ValidatedRequest.BindingMessage
        };

        var requestId = await BackChannelAuthenticationRequestStore.CreateRequestAsync(request);

        var interval = validationResult.ValidatedRequest.Client.PollingInterval ?? Options.Ciba.DefaultPollingInterval;
        var response = new BackchannelAuthenticationResponse
        {
            AuthenticationRequestId = requestId,
            ExpiresIn = request.Lifetime,
            Interval = interval
        };

        await UserLoginService.SendLoginRequestAsync(new BackchannelUserLoginRequest
        {
            InternalId = request.InternalId,
            Subject = validationResult.ValidatedRequest.Subject,
            Client = validationResult.ValidatedRequest.Client,
            ValidatedResources = validationResult.ValidatedRequest.ValidatedResources,
            RequestedResourceIndicators = validationResult.ValidatedRequest.RequestedResourceIndicators,
            BindingMessage = validationResult.ValidatedRequest.BindingMessage,
            AuthenticationContextReferenceClasses = validationResult.ValidatedRequest.AuthenticationContextReferenceClasses,
            Tenant = validationResult.ValidatedRequest.Tenant,
            IdP = validationResult.ValidatedRequest.IdP,
        });

        return response;
    }
}