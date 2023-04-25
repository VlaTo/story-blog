using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SlimMessageBus;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Events;
using StoryBlog.Web.Microservices.Posts.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Application.MessageBus.Handlers;

public sealed class BlogCommentEventHandler : IConsumer<BlogCommentEvent>
{
    private readonly IUnitOfWork context;
    private readonly ILogger<BlogCommentEventHandler> logger;

    public BlogCommentEventHandler(
        IUnitOfWork context,
        ILogger<BlogCommentEventHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task OnHandle(BlogCommentEvent message)
    {
        logger.LogInformation($"Received event: action = {message.Action} for key: \"{message.Key}\"");

        switch (message.Action)
        {
            case BlogCommentAction.Added:
            {
                if (false == await UpdateCommentsCountAsync(message.PostKey, 1))
                {
                    logger.LogWarning($"No post found for key: {message.PostKey}");
                }
                
                break;
            }

            case BlogCommentAction.Deleted:
            {
                //return DeletePostCommentsAsync(message.Key);
                break;
            }

            default:
            {
                break;
            }
        }
    }

    private async Task<bool> UpdateCommentsCountAsync(Guid key, int delta)
    {
        await using (var repository = context.GetRepository<Domain.Entities.Post>())
        {
            var post = await repository.FindAsync(new FindPostByKeySpecification(key));

            if (null == post)
            {
                return false;
            }

            post.CommentsCounter.Counter += delta;

            await repository.SaveChangesAsync();
        }

        return true;
    }
}