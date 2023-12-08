using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Application.Stores;
using StoryBlog.Web.Microservices.Identity.Infrastructure.Extensions;
using StoryBlog.Web.Microservices.Identity.Infrastructure.Specifications;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Stores;

/// <summary>
/// Implementation of IClientStore thats uses EF.
/// </summary>
/// <seealso cref="IClientStore" />
public class ClientStore : IClientStore
{
    /// <summary>
    /// The DbContext.
    /// </summary>
    protected readonly IAsyncUnitOfWork Context;

    /// <summary>
    /// The CancellationToken provider.
    /// </summary>
    protected readonly ICancellationTokenProvider CancellationTokenProvider;

    /// <summary>
    /// The logger.
    /// </summary>
    protected readonly ILogger<ClientStore> Logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientStore"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="cancellationTokenProvider"></param>
    /// <exception cref="ArgumentNullException">context</exception>
    public ClientStore(
        IAsyncUnitOfWork context,
        ICancellationTokenProvider cancellationTokenProvider,
        ILogger<ClientStore> logger)
    {
        Context = context;
        CancellationTokenProvider = cancellationTokenProvider;
        Logger = logger;
    }

    /// <summary>
    /// Finds a client by id
    /// </summary>
    /// <param name="clientId">The client id</param>
    /// <returns>
    /// The client
    /// </returns>
    public virtual async Task<Application.Storage.Client?> FindClientByIdAsync(string clientId)
    {
        using (var activity = Tracing.StoreActivitySource.StartActivity("ClientStore.FindClientById"))
        {
            activity?.SetTag(Tracing.Properties.ClientId, clientId);

            try
            {
                await using (var repository = Context.GetRepository<Domain.Entities.Client>())
                {
                    var specification = new FindClientById(clientId);
                    var client = await repository.FindAsync(specification, CancellationTokenProvider.CancellationToken);

                    if (null != client)
                    {
                        var model = client.ToModel();

                        Logger.LogDebug("Client {0} found in database", clientId);

                        return model;
                    }

                    Logger.LogDebug("Client {0} not found in database", clientId);
                }
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "Error while getting client {0}", clientId);
            }
        }

        return null;
    }
}