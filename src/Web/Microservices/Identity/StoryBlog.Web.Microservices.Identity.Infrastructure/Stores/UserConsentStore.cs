using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Application;
using StoryBlog.Web.Microservices.Identity.Application.Storage;
using StoryBlog.Web.Microservices.Identity.Application.Stores;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Stores;

/// <summary>
/// Default user consent store.
/// </summary>
public class UserConsentStore : GrantStore<Consent>, IUserConsentStore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultUserConsentStore"/> class.
    /// </summary>
    /// <param name="store">The store.</param>
    /// <param name="serializer">The serializer.</param>
    /// <param name="handleGenerationService">The handle generation service.</param>
    /// <param name="logger">The logger.</param>
    public UserConsentStore(
        IPersistedGrantStore store,
        IPersistentGrantSerializer serializer,
        IHandleGenerationService handleGenerationService,
        ILogger<UserConsentStore> logger)
        : base(
            IdentityServerConstants.PersistedGrantTypes.UserConsent,
            store,
            serializer,
            handleGenerationService,
            logger)
    {
    }

    /// <summary>
    /// Stores the user consent asynchronous.
    /// </summary>
    /// <param name="consent">The consent.</param>
    /// <returns></returns>
    public Task StoreUserConsentAsync(Consent consent)
    {
        using var activity = Tracing.ActivitySource.StartActivity("DefaultUserConsentStore.StoreUserConsent");

        var key = GetConsentKey(consent.SubjectId, consent.ClientId);
        return StoreItemAsync(key, consent, consent.ClientId, consent.SubjectId, null, null, consent.CreationTime, consent.Expiration);
    }

    /// <summary>
    /// Gets the user consent asynchronous.
    /// </summary>
    /// <param name="subjectId">The subject identifier.</param>
    /// <param name="clientId">The client identifier.</param>
    /// <returns></returns>
    public Task<Consent?> GetUserConsentAsync(string subjectId, string clientId)
    {
        using var activity = Tracing.ActivitySource.StartActivity("DefaultUserConsentStore.GetUserConsent");

        var key = GetConsentKey(subjectId, clientId);

        return GetItemAsync(key);
    }

    /// <summary>
    /// Removes the user consent asynchronous.
    /// </summary>
    /// <param name="subjectId">The subject identifier.</param>
    /// <param name="clientId">The client identifier.</param>
    /// <returns></returns>
    public Task RemoveUserConsentAsync(string subjectId, string clientId)
    {
        using var activity = Tracing.ActivitySource.StartActivity("DefaultUserConsentStore.RemoveUserConsent");

        var key = GetConsentKey(subjectId, clientId);
        return RemoveItemAsync(key);
    }

    private static string GetConsentKey(string subjectId, string clientId)
    {
        return clientId + "|" + subjectId;
    }
}