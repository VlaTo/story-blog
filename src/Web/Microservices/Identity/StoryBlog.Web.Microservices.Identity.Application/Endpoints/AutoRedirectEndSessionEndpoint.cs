using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Endpoints.Results;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Hosting;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Application.Validation;
using System.Collections.Specialized;
using System.Net;

namespace StoryBlog.Web.Microservices.Identity.Application.Endpoints;

internal sealed class AutoRedirectEndSessionEndpoint : IEndpointHandler
{
    private readonly ILogger logger;
    private readonly IUserSession session;
    private readonly IOptions<IdentityServerOptions> identityServerOptions;
    private readonly IEndSessionRequestValidator validator;

    public AutoRedirectEndSessionEndpoint(
        IEndSessionRequestValidator validator,
        IOptions<IdentityServerOptions> identityServerOptions,
        IUserSession session,
        ILogger<AutoRedirectEndSessionEndpoint> logger)
    {
        this.logger = logger;
        this.session = session;
        this.identityServerOptions = identityServerOptions;
        this.validator = validator;
    }

    public async Task<IEndpointResult?> ProcessAsync(HttpContext ctx)
    {
        var validateRequest = ValidateRequest(ctx.Request);

        if (null != validateRequest)
        {
            return validateRequest;
        }

        var parameters = await GetParametersAsync(ctx.Request);
        var user = await session.GetUserAsync();
        var result = await validator.ValidateAsync(parameters, user);
        var options = identityServerOptions.Value;

        if (result.IsError)
        {
            logger.LogError(LoggerEventIds.EndingSessionFailed, "Error ending session {Error}", result.Error);
            return new RedirectResult(options.UserInteraction.ErrorUrl);
        }

        var client = result.ValidatedRequest.Client;

        if (null != client && client.Properties.TryGetValue(ApplicationProfilesPropertyNames.Profile, out _))
        {
            var signInScheme = options.Authentication.CookieAuthenticationScheme;

            if (null != signInScheme)
            {
                await ctx.SignOutAsync(signInScheme);
            }
            else
            {
                await ctx.SignOutAsync();
            }

            var postLogOutUri = result.ValidatedRequest.PostLogOutUri;

            if (null != result.ValidatedRequest.State)
            {
                postLogOutUri = QueryHelpers.AddQueryString(postLogOutUri, OpenIdConnectParameterNames.State, result.ValidatedRequest.State);
            }

            return new RedirectResult(postLogOutUri);
        }

        return new RedirectResult(options.UserInteraction.LogoutUrl);
    }

    private static async Task<NameValueCollection> GetParametersAsync(HttpRequest request)
    {
        if (HttpMethods.IsGet(request.Method))
        {
            return request.Query.AsNameValueCollection();
        }

        var form = await request.ReadFormAsync();

        return form.AsNameValueCollection();
    }

    private static IEndpointResult? ValidateRequest(HttpRequest request)
    {
        const string formUlrEncoded = "application/x-www-form-urlencoded";

        if (false == HttpMethods.IsPost(request.Method) && false == HttpMethods.IsGet(request.Method))
        {
            return new StatusCodeResult(HttpStatusCode.BadRequest);
        }

        if (HttpMethods.IsPost(request.Method) && false == String.Equals(request.ContentType, formUlrEncoded, StringComparison.OrdinalIgnoreCase))
        {
            return new StatusCodeResult(HttpStatusCode.BadRequest);
        }

        return null;
    }

    #region RedirectResult

    internal sealed class RedirectResult : IEndpointResult
    {
        public string Url
        {
            get;
        }

        public RedirectResult(string url)
        {
            Url = url;
        }

        public Task ExecuteAsync(HttpContext context)
        {
            context.Response.Redirect(Url);
            return Task.CompletedTask;
        }
    }

    #endregion
}