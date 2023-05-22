using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Application.Storage;
using StoryBlog.Web.Microservices.Identity.Application.Stores;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Requests;
using StoryBlog.Web.Microservices.Identity.Infrastructure.Extensions;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Stores;

/// <summary>
/// Default authorization code store.
/// </summary>
public class DefaultBackChannelAuthenticationRequestStore : GrantStore<BackChannelAuthenticationRequest>, IBackChannelAuthenticationRequestStore
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultAuthorizationCodeStore"/> class.
    /// </summary>
    /// <param name="store">The store.</param>
    /// <param name="serializer">The serializer.</param>
    /// <param name="handleGenerationService">The handle generation service.</param>
    /// <param name="logger">The logger.</param>
    public DefaultBackChannelAuthenticationRequestStore(
        IPersistedGrantStore store,
        IPersistentGrantSerializer serializer,
        IHandleGenerationService handleGenerationService,
        ILogger<DefaultBackChannelAuthenticationRequestStore> logger)
        : base(
            IdentityServerConstants.PersistedGrantTypes.BackChannelAuthenticationRequest,
            store,
            serializer,
            handleGenerationService,
            logger)
    {
    }

    /// <inheritdoc cref="CreateRequestAsync" />
    public async Task<string> CreateRequestAsync(BackChannelAuthenticationRequest request)
    {
        using var activity = Tracing.StoreActivitySource.StartActivity("DefaultBackChannelAuthenticationRequestStore.CreateRequest");

        var handle = await CreateHandleAsync();

        request.InternalId = GetHashedKey(handle);

        await StoreItemByHashedKeyAsync(
            request.InternalId,
            request,
            request.ClientId,
            request.Subject.GetSubjectId(),
            null,
            null,
            request.CreationTime,
            request.CreationTime + request.Lifetime
        );

        return handle;
    }

    /// <inheritdoc cref="GetLoginsForUserAsync" />
    public Task<IEnumerable<BackChannelAuthenticationRequest>?> GetLoginsForUserAsync(string subjectId, string? clientId = null)
    {
        using var activity = Tracing.StoreActivitySource.StartActivity("DefaultBackChannelAuthenticationRequestStore.GetLoginsForUser");

        return GetAllAsync(new PersistedGrantFilter(subjectId: subjectId, clientId: clientId, type: GrantType));
    }

    /// <inheritdoc cref="GetByAuthenticationRequestIdAsync" />
    public Task<BackChannelAuthenticationRequest?> GetByAuthenticationRequestIdAsync(string requestId)
    {
        using var activity = Tracing.StoreActivitySource.StartActivity("DefaultBackChannelAuthenticationRequestStore.GetByAuthenticationRequestId");

        return GetItemAsync(requestId);
    }

    /// <inheritdoc cref="GetByInternalIdAsync" />
    public Task<BackChannelAuthenticationRequest?> GetByInternalIdAsync(string id)
    {
        using var activity = Tracing.StoreActivitySource.StartActivity("DefaultBackChannelAuthenticationRequestStore.GetByInternalId");

        return GetItemByHashedKeyAsync(id);
    }

    /// <inheritdoc cref="RemoveByInternalIdAsync" />
    public Task RemoveByInternalIdAsync(string requestId)
    {
        using var activity = Tracing.StoreActivitySource.StartActivity("DefaultBackChannelAuthenticationRequestStore.RemoveByInternalId");

        return RemoveItemByHashedKeyAsync(requestId);
    }

    /// <inheritdoc cref="UpdateByInternalIdAsync" />
    public Task UpdateByInternalIdAsync(string id, BackChannelAuthenticationRequest request)
    {
        using var activity = Tracing.StoreActivitySource.StartActivity("DefaultBackChannelAuthenticationRequestStore.UpdateByInternalId");

        return StoreItemByHashedKeyAsync(
            id,
            request,
            request.ClientId,
            request.Subject.GetSubjectId(),
            request.SessionId,
            request.Description,
            request.CreationTime,
            request.CreationTime + request.Lifetime
        );
    }
}