using StoryBlog.Web.Common.Domain.Repositories;
using StoryBlog.Web.Common.Domain.Specifications;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;
using StoryBlog.Web.Microservices.Posts.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Application.Extensions;

internal static class PostHandlerExtensions
{
    public static async Task<Post?> FindPostBySlugOrKeyAsync(
        this IAsyncGenericRepository<Post> repository,
        string slugOrKey,
        bool includeAll = true,
        CancellationToken cancellationToken = default)
    {
        var pass = 0;
        ISpecification<Post> specification = new FindPostBySlugSpecification(slugOrKey, includeAll: includeAll);

        while (true)
        {
            var entity = await repository
                .FindAsync(specification, cancellationToken)
                .ConfigureAwait(false);

            if (null != entity)
            {
                return entity;
            }

            if (0 == pass)
            {
                if (false == Guid.TryParse(slugOrKey, out var key))
                {
                    break;
                }

                specification = new FindPostByKeySpecification(key, includeAll: includeAll);
                pass++;
            }
            else
            {
                break;
            }
        }

        return null;
    }
}