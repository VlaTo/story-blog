using Microsoft.Extensions.Logging;

namespace StoryBlog.Web.Microservices.Identity.Application.Services;

public sealed class CorsPolicyService : ICorsPolicyService
{
    private readonly IServiceProvider provider;
    private readonly ICancellationTokenProvider cancellationTokenProvider;
    private readonly ILogger<CorsPolicyService> logger;

    public CorsPolicyService(
        IServiceProvider provider,
        ICancellationTokenProvider cancellationTokenProvider,
        ILogger<CorsPolicyService> logger)
    {
        this.provider = provider;
        this.cancellationTokenProvider = cancellationTokenProvider;
        this.logger = logger;
    }

    public Task<bool> IsOriginAllowedAsync(string origin)
    {
        origin = origin.ToLowerInvariant();

        var isAllowed = true;

        // doing this here and not in the ctor because: https://github.com/aspnet/CORS/issues/105
        /*var dbContext = provider.GetRequiredService<IConfigurationDbContext>();



        var query = from o in dbContext.ClientCorsOrigins
            where o.Origin == origin
            select o;

        var isAllowed = await query.AnyAsync(cancellationTokenProvider.CancellationToken);*/

        logger.LogDebug("Origin {origin} is allowed: {originAllowed}", origin, isAllowed);

        return Task.FromResult(isAllowed);
    }
}