using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Application.Storage;
using StoryBlog.Web.Microservices.Identity.Application.Stores;
using StoryBlog.Web.Microservices.Identity.Infrastructure.Extensions;
using StoryBlog.Web.Microservices.Identity.Infrastructure.Specifications;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Stores;

/// <summary>
/// Implementation of IResourceStore thats uses EF.
/// </summary>
/// <seealso cref="IResourceStore" />
public class ResourceStore : IResourceStore
{
    /// <summary>
    /// The DbContext.
    /// </summary>
    protected readonly IUnitOfWork Context;

    /// <summary>
    /// The CancellationToken provider.
    /// </summary>
    protected readonly ICancellationTokenProvider CancellationTokenProvider;

    /// <summary>
    /// The logger.
    /// </summary>
    protected readonly ILogger<ResourceStore> Logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceStore"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="cancellationTokenProvider"></param>
    /// <exception cref="ArgumentNullException">context</exception>
    public ResourceStore(
        IUnitOfWork context,
        ILogger<ResourceStore> logger,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        Logger = logger;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    /// <summary>
    /// Finds the API resources by name.
    /// </summary>
    /// <param name="apiResourceNames">The names.</param>
    /// <returns></returns>
    public virtual async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
    {
        using (var activity = Tracing.StoreActivitySource.StartActivity("ResourceStore.FindApiResourcesByName"))
        {
            activity?.SetTag(Tracing.Properties.ApiResourceNames, apiResourceNames.ToSpaceSeparatedString());

            using (var repository = Context.GetRepository<Domain.Entities.ApiResource>())
            {
                var specification = new FindApiResourcesByName(apiResourceNames.ToArray());
                var result = await repository.QueryAsync(specification, CancellationTokenProvider.CancellationToken);
                var resources = result
                    .Select(resource => resource.ToModel())
                    .ToArray();

                if (0 < resources.Length)
                {
                    Logger.LogDebug("Found {apis} API resource in database", result.Select(x => x.Name));
                }
                else
                {
                    Logger.LogDebug("Did not find {apis} API resource in database", apiResourceNames);
                }

                return resources;
            }
        }
    }

    public Task<Resources> GetAllResourcesAsync()
    {
        using var activity = Tracing.StoreActivitySource.StartActivity("ResourceStore.GetAllResources");

        /*var identity = await Context.IdentityResources
            .Include(x => x.UserClaims)
            .Include(x => x.Properties)
            .AsNoTracking()
            .ToArrayAsync(CancellationTokenProvider.CancellationToken);

        var apis = await Context.ApiResources
            .Include(x => x.Secrets)
            .Include(x => x.Scopes)
            .Include(x => x.UserClaims)
            .Include(x => x.Properties)
            .AsNoTracking()
            .ToArrayAsync(CancellationTokenProvider.CancellationToken);

        var scopes = await Context.ApiScopes
            .Include(x => x.UserClaims)
            .Include(x => x.Properties)
            .AsNoTracking()
            .ToArrayAsync(CancellationTokenProvider.CancellationToken);

        var result = Resources.Create(
            identity.Select(resource => resource.ToModel()),
            apis.Select(resource => resource.ToModel()),
            scopes.Select(resource => resource.ToModel())
        );

        Logger.LogDebug(
            "Found {scopes} as all scopes, and {apis} as API resources",
            result.IdentityResources
                .Select(x => x.Name)
                .Union(result.ApiScopes.Select(x => x.Name)),
            result.ApiResources.Select(x => x.Name)
        );

        return result;*/

        return Task.FromResult(new Resources());
    }

    /// <summary>
    /// Gets identity resources by scope name.
    /// </summary>
    /// <param name="scopeNames"></param>
    /// <returns></returns>
    public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
    {
        using (var activity = Tracing.StoreActivitySource.StartActivity("ResourceStore.FindIdentityResourcesByScopeName"))
        {
            activity?.SetTag(Tracing.Properties.ScopeNames, scopeNames.ToSpaceSeparatedString());

            using (var repository = Context.GetRepository<Domain.Entities.IdentityResource>())
            {
                var specification = new FindIdentityResourcesByScopes(scopeNames.ToArray());
                var resources = await repository.QueryAsync(specification, CancellationTokenProvider.CancellationToken);

                Logger.LogDebug("Found {scopes} identity scopes in database", resources.Select(x => x.Name));

                return resources
                    .Select(x => x.ToModel())
                    .ToArray();
            }
        }
    }

    public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
    {
        using (var activity = Tracing.StoreActivitySource.StartActivity("ResourceStore.FindApiScopesByName"))
        {
            activity?.SetTag(Tracing.Properties.ScopeNames, scopeNames.ToSpaceSeparatedString());

            using (var repository = Context.GetRepository<Domain.Entities.ApiScope>())
            {
                var specification = new FindApiScopesByScopes(scopeNames.ToArray());
                var resources = await repository.QueryAsync(specification, CancellationTokenProvider.CancellationToken);

                Logger.LogDebug("Found {scopes} identity scopes in database", resources.Select(x => x.Name));

                return resources
                    .Select(x => x.ToModel())
                    .ToArray();
            }
        }
    }

    /// <summary>
    /// Gets API resources by scope name.
    /// </summary>
    /// <param name="scopeNames"></param>
    /// <returns></returns>
    public virtual async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
    {
        using (var activity = Tracing.StoreActivitySource.StartActivity("ResourceStore.FindApiResourcesByScopeName"))
        {
            activity?.SetTag(Tracing.Properties.ScopeNames, scopeNames.ToSpaceSeparatedString());

            using (var repository = Context.GetRepository<Domain.Entities.ApiResource>())
            {
                var specification = new FindApiResourcesByScopeNames(scopeNames.ToArray());
                var resources = await repository.QueryAsync(specification, CancellationTokenProvider.CancellationToken);
                var models = resources
                    .Select(x => x.ToModel())
                    .ToArray();

                Logger.LogDebug("Found {apis} API resources in database", models.Select(x => x.Name));

                return models;
            }
        }
    }
}