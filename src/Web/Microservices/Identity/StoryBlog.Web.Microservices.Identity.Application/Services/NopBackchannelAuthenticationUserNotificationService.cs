using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application.Models.Requests;

namespace StoryBlog.Web.Microservices.Identity.Application.Services;

/// <summary>
/// Nop implementation of IUserLoginService.
/// </summary>
public sealed class NopBackchannelAuthenticationUserNotificationService : IBackchannelAuthenticationUserNotificationService
{
    private readonly IIssuerNameService issuerNameService;
    private readonly ILogger<NopBackchannelAuthenticationUserNotificationService> logger;

    /// <summary>
    /// Ctor
    /// </summary>
    public NopBackchannelAuthenticationUserNotificationService(
        IIssuerNameService issuerNameService,
        ILogger<NopBackchannelAuthenticationUserNotificationService> logger)
    {
        this.issuerNameService = issuerNameService;
        this.logger = logger;
    }

    /// <inheritdoc/>
    public async Task SendLoginRequestAsync(BackchannelUserLoginRequest request)
    {
        var url = await issuerNameService.GetCurrentAsync();
        url += "/ciba?id=" + request.InternalId;
        logger.LogWarning("IBackchannelAuthenticationUserNotificationService not implemented. But for testing, visit {url} to simulate what a user might need to do to complete the request.", url);
    }
}