﻿using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Microservices.Identity.Application.Authorization.Responses;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Hosting;
using StoryBlog.Web.Microservices.Identity.Application.Models.Messages;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Application.Stores;
using System.Text.Encodings.Web;

namespace StoryBlog.Web.Microservices.Identity.Application.Endpoints.Results;

internal class AuthorizeResult : IEndpointResult
{
    private IdentityServerOptions? options;
    private IUserSession? userSession;
    private IMessageStore<ErrorMessage>? errorMessageStore;
    private IServerUrls? urls;
    private TimeProvider? clock;

    public AuthorizeResponse Response
    {
        get;
    }

    public AuthorizeResult(AuthorizeResponse response)
    {
        Response = response ?? throw new ArgumentNullException(nameof(response));
    }

    internal AuthorizeResult(
        AuthorizeResponse response,
        IdentityServerOptions options,
        IUserSession userSession,
        IMessageStore<ErrorMessage> errorMessageStore,
        IServerUrls urls,
        TimeProvider clock)
        : this(response)
    {
        this.options = options;
        this.userSession = userSession;
        this.errorMessageStore = errorMessageStore;
        this.urls = urls;
        this.clock = clock;
    }

    public async Task ExecuteAsync(HttpContext context)
    {
        Init(context);

        if (Response.IsError)
        {
            await ProcessErrorAsync(context);
        }
        else
        {
            await ProcessResponseAsync(context);
        }
    }

    protected async Task ProcessResponseAsync(HttpContext context)
    {
        if (false == Response.IsError)
        {
            // success response -- track client authorization for sign-out
            //_logger.LogDebug("Adding client {0} to client list cookie for subject {1}", request.ClientId, request.Subject.GetSubjectId());
            await userSession!.AddClientIdAsync(Response.Request!.ClientId!);
        }

        await RenderAuthorizeResponseAsync(context);
    }

    private async Task ProcessErrorAsync(HttpContext context)
    {
        // these are the conditions where we can send a response 
        // back directly to the client, otherwise we're only showing the error UI
        var isSafeError =
            Response.Error == OidcConstants.AuthorizeErrors.AccessDenied ||
            Response.Error == OidcConstants.AuthorizeErrors.AccountSelectionRequired ||
            Response.Error == OidcConstants.AuthorizeErrors.LoginRequired ||
            Response.Error == OidcConstants.AuthorizeErrors.ConsentRequired ||
            Response.Error == OidcConstants.AuthorizeErrors.InteractionRequired ||
            Response.Error == OidcConstants.AuthorizeErrors.TemporarilyUnavailable;

        if (isSafeError)
        {
            // this scenario we can return back to the client
            await ProcessResponseAsync(context);
        }
        else
        {
            // we now know we must show error page
            await RedirectToErrorPageAsync(context);
        }
    }

    private void Init(HttpContext context)
    {
        options ??= context.RequestServices.GetRequiredService<IdentityServerOptions>();
        userSession ??= context.RequestServices.GetRequiredService<IUserSession>();
        errorMessageStore ??= context.RequestServices.GetRequiredService<IMessageStore<ErrorMessage>>();
        urls ??= context.RequestServices.GetRequiredService<IServerUrls>();
        clock ??= context.RequestServices.GetRequiredService<TimeProvider>();
    }

    private async Task RenderAuthorizeResponseAsync(HttpContext context)
    {
        if (OidcConstants.ResponseModes.Query == Response.Request?.ResponseMode ||
            OidcConstants.ResponseModes.Fragment == Response.Request?.ResponseMode)
        {
            context.Response.SetNoCache();
            context.Response.Redirect(BuildRedirectUri());
        }
        else if (Response.Request?.ResponseMode == OidcConstants.ResponseModes.FormPost)
        {
            context.Response.SetNoCache();
            AddSecurityHeaders(context);
            await context.Response.WriteHtmlAsync(GetFormPostHtml());
        }
        else
        {
            //_logger.LogError("Unsupported response mode.");
            throw new InvalidOperationException("Unsupported response mode");
        }
    }

    private void AddSecurityHeaders(HttpContext context)
    {
        context.Response.AddScriptCspHeaders(options!.Csp, "sha256-orD0/VhH8hLqrLxKHD/HUEMdwqX6/0ve7c5hspX5VJ8=");

        if (false == context.Response.Headers.ContainsKey(HeaderNames.ReferrerPolicy))
        {
            context.Response.Headers.Append(HeaderNames.ReferrerPolicy, "no-referrer");
        }
    }

    private string BuildRedirectUri()
    {
        var uri = Response.RedirectUri;
        var query = Response.ToNameValueCollection().ToQueryString();

        uri = OidcConstants.ResponseModes.Query == Response.Request?.ResponseMode
            ? uri!.AddQueryString(query)
            : uri!.AddHashFragment(query);

        if (Response.IsError && !uri.Contains(UriSymbols.HashMark))
        {
            // https://tools.ietf.org/html/draft-bradley-oauth-open-redirector-00
            uri += "#_=_";
        }

        return uri;
    }

    private string GetFormPostHtml()
    {
        var html = FormPostHtml;

        var url = Response.Request?.RedirectUri;
        
        if (null != url)
        {
            url = HtmlEncoder.Default.Encode(url);
            html = html.Replace("{uri}", url);
            html = html.Replace("{body}", Response.ToNameValueCollection().ToFormPost());
        }

        return html;
    }

    private async Task RedirectToErrorPageAsync(HttpContext context)
    {
        var errorModel = new ErrorMessage
        {
            RequestId = context.TraceIdentifier,
            Error = Response.Error!,
            ErrorDescription = Response.ErrorDescription!,
            UiLocales = Response.Request!.UiLocales,
            DisplayMode = Response.Request.DisplayMode,
            ClientId = Response.Request.ClientId!
        };

        if (Response is { RedirectUri: not null, Request.ResponseMode: not null })
        {
            // if we have a valid redirect uri, then include it to the error page
            errorModel.RedirectUri = BuildRedirectUri();
            errorModel.ResponseMode = Response.Request.ResponseMode;
        }

        var message = new Message<ErrorMessage>(errorModel, clock!.GetUtcNow().UtcDateTime);
        var id = await errorMessageStore!.WriteAsync(message);
        var errorUrl = options!.UserInteraction.ErrorUrl;
        var url = errorUrl?.AddQueryString(options!.UserInteraction.ErrorIdParameter!, id);

        context.Response.Redirect(urls!.GetAbsoluteUrl(url));
    }
    
    #region Private

    private const string FormPostHtml = "<html><head><meta http-equiv='X-UA-Compatible' content='IE=edge' /><base target='_self'/></head><body><form method='post' action='{uri}'>{body}<noscript><button>Click to continue</button></noscript></form><script>window.addEventListener('load', function(){document.forms[0].submit();});</script></body></html>";

    #endregion
}