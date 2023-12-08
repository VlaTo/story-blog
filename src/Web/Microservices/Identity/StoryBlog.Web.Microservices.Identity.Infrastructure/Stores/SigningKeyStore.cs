using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Application.Storage;
using StoryBlog.Web.Microservices.Identity.Application.Stores;
using StoryBlog.Web.Microservices.Identity.Domain.Entities;
using StoryBlog.Web.Microservices.Identity.Infrastructure.Specifications;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Stores;

/// <summary>
/// Implementation of ISigningKeyStore thats uses EF.
/// </summary>
/// <seealso cref="ISigningKeyStore" />
public class SigningKeyStore : ISigningKeyStore
{
    const string Use = "signing";

    /// <summary>
    /// The DbContext.
    /// </summary>
    protected IAsyncUnitOfWork Context
    {
        init;
        get;
    }

    /// <summary>
    /// The CancellationToken provider.
    /// </summary>
    protected ICancellationTokenProvider CancellationTokenProvider
    {
        init;
        get;
    }

    /// <summary>
    /// The logger.
    /// </summary>
    protected ILogger<SigningKeyStore> Logger
    {
        init;
        get;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SigningKeyStore"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="cancellationTokenProvider"></param>
    /// <exception cref="ArgumentNullException">context</exception>
    public SigningKeyStore(
        IAsyncUnitOfWork context,
        ICancellationTokenProvider cancellationTokenProvider,
        ILogger<SigningKeyStore> logger)
    {
        Context = context;
        Logger = logger;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    /// <summary>
    /// Loads all keys from store.
    /// </summary>
    /// <returns></returns>
    public async Task<IReadOnlyCollection<SerializedKey>> LoadKeysAsync()
    {
        using (Tracing.ActivitySource.StartActivity("SigningKeyStore.LoadKeys"))
        {
            Key[] entities;

            await using (var repository = Context.GetRepository<Key>())
            {
                var specification = new QueryKeysByUse(Use);
                
                entities = await repository.QueryAsync(
                    specification,
                    CancellationTokenProvider.CancellationToken
                );
            }

            return entities
                .Select(key => new SerializedKey
                {
                    Id = key.Id,
                    Created = key.Created,
                    Version = key.Version,
                    Algorithm = key.Algorithm,
                    Data = key.Data,
                    DataProtected = key.DataProtected,
                    IsX509Certificate = key.IsX509Certificate
                })
                .ToArray();
        }
    }

    /// <summary>
    /// Persists new key in store.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task StoreKeyAsync(SerializedKey key)
    {
        using (Tracing.ActivitySource.StartActivity("SigningKeyStore.StoreKey"))
        {
            var entity = new Key
            {
                Id = key.Id,
                Use = Use,
                Created = key.Created,
                Version = key.Version,
                Algorithm = key.Algorithm,
                Data = key.Data,
                DataProtected = key.DataProtected,
                IsX509Certificate = key.IsX509Certificate
            };

            await using (var repository = Context.GetRepository<Key>())
            {
                await repository.AddAsync(entity, CancellationTokenProvider.CancellationToken);
                await repository.SaveChangesAsync(CancellationTokenProvider.CancellationToken);
            }
        }
    }

    /// <summary>
    /// Deletes key from storage.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteKeyAsync(string id)
    {
        using (Tracing.ActivitySource.StartActivity("SigningKeyStore.DeleteKey"))
        {
            await using (var repository = Context.GetRepository<Key>())
            {
                var query = new FindKeyByIdAndUse(id, Use);
                var key = await repository.FindAsync(query, CancellationTokenProvider.CancellationToken);

                if (null != key)
                {
                    await repository.RemoveAsync(key, CancellationTokenProvider.CancellationToken);
                }

                await repository.SaveChangesAsync(CancellationTokenProvider.CancellationToken);
            }
        }
    }
}