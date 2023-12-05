using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Domain.Repositories;
using StoryBlog.Web.Common.Domain.Specifications;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;
using StoryBlog.Web.Microservices.Posts.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers;

public class PostHandlerBase : HandlerBase
{
    public ILogger Logger
    {
        get;
    }

    protected PostHandlerBase(ILogger logger)
    {
        Logger = logger;
    }

    protected async Task<Post?> FindPostAsync(
        IAsyncGenericRepository<Post> repository,
        string slugOrKey,
        CancellationToken cancellationToken)
    {
        var step = 0;
        ISpecification<Post> specification = new FindPostBySlugSpecification(slugOrKey);

        Logger.LogDebug($"FindPostBySlugSpecification(slug=\"{slugOrKey}\")");

        while (true)
        {
            var entity = await repository
                .FindAsync(specification, cancellationToken)
                .ConfigureAwait(false);

            if (null != entity)
            {
                return entity;
            }

            if (0 == step)
            {
                if (false == Guid.TryParse(slugOrKey, out var key))
                {
                    Logger.LogWarning($"Failed to parse Guid from \"{slugOrKey}\"");

                    break;
                }

                Logger.LogDebug($"FindPostByKeySpecification(key=\"{key}\")");

                specification = new FindPostByKeySpecification(key, includeAll: true);
                step++;
            }
            else
            {
                break;
            }
        }

        return null;
    }
}