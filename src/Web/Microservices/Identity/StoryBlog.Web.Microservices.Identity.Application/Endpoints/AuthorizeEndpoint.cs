using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Hosting;
using StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Generators;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Application.Stores;
using System.Collections.Specialized;
using System.Net;
using StoryBlog.Web.Microservices.Identity.Application.Endpoints.Results;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;

namespace StoryBlog.Web.Microservices.Identity.Application.Endpoints;

internal class AuthorizeEndpoint : AuthorizeEndpointBase
{
    public AuthorizeEndpoint(
        IEventService events,
        ILogger<AuthorizeEndpoint> logger,
        IdentityServerOptions options,
        IAuthorizeRequestValidator validator,
        IAuthorizeInteractionResponseGenerator interactionGenerator,
        IAuthorizeResponseGenerator authorizeResponseGenerator,
        IUserSession userSession,
        IConsentMessageStore consentResponseStore,
        IAuthorizationParametersMessageStore? authorizationParametersMessageStore = null)
        : base(
            events,
            logger,
            options,
            validator,
            interactionGenerator,
            authorizeResponseGenerator,
            userSession,
            consentResponseStore,
            authorizationParametersMessageStore)
    {
    }

    public override async Task<IEndpointResult?> ProcessAsync(HttpContext context)
    {
        using var activity = Tracing.ActivitySource.StartActivity(Constants.EndpointNames.Authorize + "Endpoint");
        // todo: add complete url?

        Logger.LogDebug("Start authorize request");

        NameValueCollection values;

        if (HttpMethods.IsGet(context.Request.Method))
        {
            values = context.Request.Query.AsNameValueCollection();
        }
        else if (HttpMethods.IsPost(context.Request.Method))
        {
            if (!context.Request.HasApplicationFormContentType())
            {
                return new StatusCodeResult(HttpStatusCode.UnsupportedMediaType);
            }

            values = context.Request.Form.AsNameValueCollection();
        }
        else
        {
            return new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
        }

        var user = await UserSession.GetUserAsync();
        var result = await ProcessAuthorizeRequestAsync(values, user);

        Logger.LogTrace("End authorize request. result type: {0}", result?.GetType().ToString() ?? "-none-");

        return result;
    }
}