using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Models.Messages;
using StoryBlog.Web.Microservices.Identity.Application.Models.Requests;
using StoryBlog.Web.Microservices.Identity.Application.Models.Responses;
using StoryBlog.Web.Microservices.Identity.Application.Models;
using StoryBlog.Web.Microservices.Identity.Application.Stores;

namespace StoryBlog.Web.Microservices.Identity.Application.Services.Defaults;

internal sealed class DefaultIdentityServerInteractionService : IIdentityServerInteractionService
{
    private readonly ISystemClock clock;
    private readonly IHttpContextAccessor context;
    private readonly IMessageStore<LogoutMessage> logoutMessageStore;
    private readonly IMessageStore<ErrorMessage> errorMessageStore;
    private readonly IConsentMessageStore consentMessageStore;
    private readonly IPersistedGrantService grants;
    private readonly IUserSession userSession;
    private readonly ILogger logger;
    private readonly ReturnUrlParser returnUrlParser;

    public DefaultIdentityServerInteractionService(
        ISystemClock clock,
        IHttpContextAccessor context,
        IMessageStore<LogoutMessage> logoutMessageStore,
        IMessageStore<ErrorMessage> errorMessageStore,
        IConsentMessageStore consentMessageStore,
        IPersistedGrantService grants,
        IUserSession userSession,
        ReturnUrlParser returnUrlParser,
        ILogger<DefaultIdentityServerInteractionService> logger)
    {
        this.clock = clock;
        this.context = context;
        this.logoutMessageStore = logoutMessageStore;
        this.errorMessageStore = errorMessageStore;
        this.consentMessageStore = consentMessageStore;
        this.grants = grants;
        this.userSession = userSession;
        this.returnUrlParser = returnUrlParser;
        this.logger = logger;
    }

    public async Task<AuthorizationRequest?> GetAuthorizationContextAsync(string? returnUrl)
    {
        using var activity = Tracing.ServiceActivitySource.StartActivity("DefaultIdentityServerInteractionService.GetAuthorizationContext");

        if (String.IsNullOrEmpty(returnUrl))
        {
            return null;
        }

        var result = await returnUrlParser.ParseAsync(returnUrl);

        if (null != result)
        {
            logger.LogTrace("AuthorizationRequest being returned");
        }
        else
        {
            logger.LogTrace("No AuthorizationRequest being returned");
        }

        return result;
    }

    public async Task<LogoutRequest> GetLogoutContextAsync(string? logoutId)
    {
        using var activity = Tracing.ServiceActivitySource.StartActivity("DefaultIdentityServerInteractionService.GetLogoutContext");

        var msg = await logoutMessageStore.ReadAsync(logoutId);
        var iframeUrl = await context.HttpContext.GetIdentityServerSignOutFrameCallbackUrlAsync(msg?.Data);
        return new LogoutRequest(iframeUrl, msg?.Data);
    }

    public async Task<string?> CreateLogoutContextAsync()
    {
        using var activity = Tracing.ServiceActivitySource.StartActivity("DefaultIdentityServerInteractionService.CreateLogoutContext");

        var user = await userSession.GetUserAsync();
        if (user != null)
        {
            var clientIds = await userSession.GetClientListAsync();
            if (clientIds.Any())
            {
                var sid = await userSession.GetSessionIdAsync();
                var msg = new Message<LogoutMessage>(new LogoutMessage
                {
                    SubjectId = user?.GetSubjectId(),
                    SessionId = sid,
                    ClientIds = clientIds
                }, clock.UtcNow.UtcDateTime);
                var id = await logoutMessageStore.WriteAsync(msg);
                return id;
            }
        }

        return null;
    }

    public async Task<ErrorMessage> GetErrorContextAsync(string errorId)
    {
        using var activity = Tracing.ServiceActivitySource.StartActivity("DefaultIdentityServerInteractionService.GetErrorContext");

        if (errorId != null)
        {
            var result = await errorMessageStore.ReadAsync(errorId);
            var data = result?.Data;
            if (data != null)
            {
                logger.LogTrace("Error context loaded");
            }
            else
            {
                logger.LogTrace("No error context found");
            }
            return data;
        }

        logger.LogTrace("No error context found");

        return null;
    }

    public async Task GrantConsentAsync(AuthorizationRequest request, ConsentResponse consent, string subject = null)
    {
        using var activity = Tracing.ServiceActivitySource.StartActivity("DefaultIdentityServerInteractionService.GrantConsent");

        if (subject == null)
        {
            var user = await userSession.GetUserAsync();
            subject = user?.GetSubjectId();
        }

        if (subject == null && consent.Granted)
        {
            throw new ArgumentNullException(nameof(subject), "User is not currently authenticated, and no subject id passed");
        }

        var consentRequest = new ConsentRequest(request, subject);
        await consentMessageStore.WriteAsync(consentRequest.Id, new Message<ConsentResponse>(consent, clock.UtcNow.UtcDateTime));
    }

    public Task DenyAuthorizationAsync(AuthorizationRequest request, AuthorizationError error, string errorDescription = null)
    {
        using var activity = Tracing.ServiceActivitySource.StartActivity("DefaultIdentityServerInteractionService.DenyAuthorization");

        var response = new ConsentResponse
        {
            Error = error,
            ErrorDescription = errorDescription
        };
        return GrantConsentAsync(request, response);
    }

    public bool IsValidReturnUrl(string returnUrl)
    {
        using var activity = Tracing.ServiceActivitySource.StartActivity("DefaultIdentityServerInteractionService.IsValidReturnUrl");

        var result = returnUrlParser.IsValidReturnUrl(returnUrl);

        if (result)
        {
            logger.LogTrace("IsValidReturnUrl true");
        }
        else
        {
            logger.LogTrace("IsValidReturnUrl false");
        }

        return result;
    }

    public async Task<IEnumerable<Grant>> GetAllUserGrantsAsync()
    {
        using var activity = Tracing.ServiceActivitySource.StartActivity("DefaultIdentityServerInteractionService.GetAllUserGrants");

        var user = await userSession.GetUserAsync();
        if (user != null)
        {
            var subject = user.GetSubjectId();
            return await grants.GetAllGrantsAsync(subject);
        }

        return Enumerable.Empty<Grant>();
    }

    public async Task RevokeUserConsentAsync(string clientId)
    {
        using var activity = Tracing.ServiceActivitySource.StartActivity("DefaultIdentityServerInteractionService.RevokeUserConsent");

        var user = await userSession.GetUserAsync();
        if (user != null)
        {
            var subject = user.GetSubjectId();
            await grants.RemoveAllGrantsAsync(subject, clientId);
        }
    }

    public async Task RevokeTokensForCurrentSessionAsync()
    {
        using var activity = Tracing.ServiceActivitySource.StartActivity("DefaultIdentityServerInteractionService.RevokeTokensForCurrentSession");

        var user = await userSession.GetUserAsync();
        if (user != null)
        {
            var subject = user.GetSubjectId();
            var sessionId = await userSession.GetSessionIdAsync();
            await grants.RemoveAllGrantsAsync(subject, sessionId: sessionId);
        }
    }
}