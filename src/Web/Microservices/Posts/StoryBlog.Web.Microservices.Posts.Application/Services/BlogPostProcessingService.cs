using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;
using StoryBlog.Web.Microservices.Posts.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Application.Services;

internal sealed class BlogPostProcessingService : IProcessor<IBackgroundTask>
{
    private readonly IAsyncUnitOfWork context;
    private readonly ILogger<BlogPostProcessingService> logger;

    public BlogPostProcessingService(
        IAsyncUnitOfWork context,
        ILogger<BlogPostProcessingService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task ProcessAsync(IBackgroundTask backgroundTask, CancellationToken cancellationToken)
    {
        await using (var repository = context.GetRepository<Post>())
        {
            var specification = new FindPostByKeySpecification(backgroundTask.PostKey, false);
            var post = await repository.FindAsync(specification, cancellationToken);

            if (null == post)
            {
                return ;
            }

            post.Content.Brief = post.Content.Text;

            await repository.SaveChangesAsync(cancellationToken);
        }
    }
}