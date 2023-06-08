using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Models;
using StoryBlog.Web.Microservices.Identity.Application.Storage;
using StoryBlog.Web.Microservices.Identity.Application.Stores;

namespace StoryBlog.Web.Microservices.Identity.Application.Services.Defaults;

/// <summary>
/// Default persisted grant service
/// </summary>
public sealed class DefaultPersistedGrantService : IPersistedGrantService
{
    private readonly ILogger logger;
    private readonly IPersistedGrantStore store;
    private readonly IPersistentGrantSerializer serializer;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultPersistedGrantService"/> class.
    /// </summary>
    /// <param name="store">The store.</param>
    /// <param name="serializer">The serializer.</param>
    /// <param name="logger">The logger.</param>
    public DefaultPersistedGrantService(
        IPersistedGrantStore store,
        IPersistentGrantSerializer serializer,
        ILogger<DefaultPersistedGrantService> logger)
    {
        this.store = store;
        this.serializer = serializer;
        this.logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Grant>> GetAllGrantsAsync(string subjectId)
    {
        using var activity = Tracing.ServiceActivitySource.StartActivity("DefaultPersistedGrantService.GetAllGrants");

        if (String.IsNullOrWhiteSpace(subjectId)) throw new ArgumentNullException(nameof(subjectId));

        var grants = await store.GetAllAsync(new PersistedGrantFilter(subjectId: subjectId));
        var actualGrants = grants
            .Where(x => null == x.ConsumedTime) // filter consumed grants
            .ToArray();

        try
        {
            var consents = actualGrants
                .Where(x => x.Type == IdentityServerConstants.PersistedGrantTypes.UserConsent)
                .Select(x => serializer.Deserialize<Consent>(x.Data))
                .Select(x => new Grant
                {
                    ClientId = x.ClientId,
                    SubjectId = subjectId,
                    Scopes = x.Scopes ?? Array.Empty<string>(),
                    CreationTime = x.CreationTime,
                    Expiration = x.Expiration
                });

            var codes = actualGrants
                .Where(x => x.Type == IdentityServerConstants.PersistedGrantTypes.AuthorizationCode)
                .Select(x => serializer.Deserialize<AuthorizationCode>(x.Data))
                .Select(x => new Grant
                {
                    ClientId = x.ClientId!,
                    SubjectId = subjectId,
                    Description = x.Description,
                    Scopes = x.RequestedScopes,
                    CreationTime = x.CreationTime,
                    Expiration = x.CreationTime + x.Lifetime
                });

            var refresh = actualGrants
                .Where(x => x.Type == IdentityServerConstants.PersistedGrantTypes.RefreshToken)
                .Select(x => serializer.Deserialize<RefreshToken>(x.Data))
                .Select(x => new Grant
                {
                    ClientId = x.ClientId,
                    SubjectId = subjectId,
                    Description = x.Description,
                    Scopes = x.AuthorizedScopes,
                    CreationTime = x.CreationTime,
                    Expiration = x.CreationTime + x.Lifetime
                });

            var access = actualGrants
                .Where(x => x.Type == IdentityServerConstants.PersistedGrantTypes.ReferenceToken)
                .Select(x => serializer.Deserialize<Token>(x.Data))
                .Select(x => new Grant
                {
                    ClientId = x.ClientId!,
                    SubjectId = subjectId,
                    Description = x.Description,
                    Scopes = x.Scopes,
                    CreationTime = x.CreationTime,
                    Expiration = x.CreationTime + x.Lifetime
                });

            consents = Join(consents, codes);
            consents = Join(consents, refresh);
            consents = Join(consents, access);

            return consents.ToArray();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed processing results from grant store.");
        }

        return Enumerable.Empty<Grant>();
    }

    /// <inheritdoc/>
    public Task RemoveAllGrantsAsync(string subjectId, string? clientId = null, string? sessionId = null)
    {
        using var activity = Tracing.ServiceActivitySource.StartActivity("DefaultPersistedGrantService.RemoveAllGrants");

        if (String.IsNullOrWhiteSpace(subjectId))
        {
            throw new ArgumentNullException(nameof(subjectId));
        }

        return store.RemoveAllAsync(new PersistedGrantFilter(subjectId: subjectId, clientId: clientId, sessionId: sessionId));
    }

    private static IEnumerable<Grant> Join(IEnumerable<Grant> first, IEnumerable<Grant> second)
    {
        var list = first.ToList();

        foreach (var other in second)
        {
            var match = list.FirstOrDefault(x => x.ClientId == other.ClientId);

            if (null != match)
            {
                match.Scopes = match.Scopes.Union(other.Scopes).Distinct();

                if (match.CreationTime > other.CreationTime)
                {
                    // show the earlier creation time
                    match.CreationTime = other.CreationTime;
                }

                if (null == match.Expiration || null == other.Expiration)
                {
                    // show that there is no expiration to one of the grants
                    match.Expiration = null;
                }
                else if (match.Expiration < other.Expiration)
                {
                    // show the latest expiration
                    match.Expiration = other.Expiration;
                }

                match.Description ??= other.Description;
            }
            else
            {
                list.Add(other);
            }
        }

        return list;
    }
}