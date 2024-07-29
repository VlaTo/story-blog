using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Models.Requests;
using StoryBlog.Web.Microservices.Identity.Application.Stores;
using System.Collections.Specialized;
using StoryBlog.Web.Common.Identity.Permission;

namespace StoryBlog.Web.Microservices.Identity.Application.Services;

internal sealed class OidcReturnUrlParser : IReturnUrlParser
{
    private readonly IdentityServerOptions options;
    private readonly IAuthorizeRequestValidator validator;
    private readonly IUserSession userSession;
    private readonly IServerUrls urls;
    private readonly ILogger logger;
    private readonly IAuthorizationParametersMessageStore? authorizationParametersMessageStore;

    public OidcReturnUrlParser(
        IdentityServerOptions options,
        IAuthorizeRequestValidator validator,
        IUserSession userSession,
        IServerUrls urls,
        ILogger<OidcReturnUrlParser> logger,
        IAuthorizationParametersMessageStore? authorizationParametersMessageStore = null)
    {
        this.options = options;
        this.validator = validator;
        this.userSession = userSession;
        this.urls = urls;
        this.logger = logger;
        this.authorizationParametersMessageStore = authorizationParametersMessageStore;
    }

    public bool IsValidReturnUrl(string? returnUrl)
    {
        using var activity = Tracing.ValidationActivitySource.StartActivity("OidcReturnUrlParser.IsValidReturnUrl");

        if (options.UserInteraction.AllowOriginInReturnUrl && null != returnUrl)
        {
            if (!Uri.IsWellFormedUriString(returnUrl, UriKind.RelativeOrAbsolute))
            {
                logger.LogTrace("returnUrl is not valid");
                return false;
            }

            var host = urls.Origin;

            if (null != host && returnUrl.StartsWith(host, StringComparison.OrdinalIgnoreCase))
            {
                returnUrl = returnUrl.Substring(host.Length);
            }
        }

        if (null != returnUrl && returnUrl.IsLocalUrl())
        {
            {
                var index = returnUrl.IndexOf('?');

                if (0 <= index)
                {
                    returnUrl = returnUrl.Substring(0, index);
                }
            }

            {
                var index = returnUrl.IndexOf('#');

                if (0 <= index)
                {
                    returnUrl = returnUrl.Substring(0, index);
                }
            }

            if (returnUrl.EndsWith(Constants.ProtocolRoutePaths.Authorize, StringComparison.Ordinal) ||
                returnUrl.EndsWith(Constants.ProtocolRoutePaths.AuthorizeCallback, StringComparison.Ordinal))
            {
                logger.LogTrace("returnUrl is valid");
                return true;
            }
        }

        logger.LogTrace("returnUrl is not valid");

        return false;
    }

    public async Task<AuthorizationRequest?> ParseAsync(string returnUrl)
    {
        using var activity = Tracing.ValidationActivitySource.StartActivity("OidcReturnUrlParser.Parse");

        if (IsValidReturnUrl(returnUrl))
        {
            var parameters = returnUrl.ReadQueryStringAsNameValueCollection();

            if (null != authorizationParametersMessageStore)
            {
                var messageStoreId = parameters[Constants.AuthorizationParamsStore.MessageStoreIdParameterName];
                var entry = await authorizationParametersMessageStore.ReadAsync(messageStoreId);
                parameters = entry?.Data.FromFullDictionary() ?? new NameValueCollection();
            }

            var user = await userSession.GetUserAsync();
            var result = await validator.ValidateAsync(parameters, user);
            
            if (false == result.IsError)
            {
                logger.LogTrace("AuthorizationRequest being returned");
                return new AuthorizationRequest(result.ValidatedRequest);
            }
        }

        logger.LogTrace("No AuthorizationRequest being returned");
        
        return null;
    }
}