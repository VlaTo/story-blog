using Microsoft.Extensions.Logging;
using SlimMessageBus;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Events;
using StoryBlog.Web.Microservices.Comments.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Comments.Application.MessageBus.Handlers;

public sealed class PostRemovedEventHandler : IConsumer<PostRemovedEvent>
{
    private readonly IAsyncUnitOfWork context;
    private readonly ILogger<NewPostCreatedEventHandler> logger;

    public PostRemovedEventHandler(
        IAsyncUnitOfWork context,
        ILogger<NewPostCreatedEventHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task OnHandle(PostRemovedEvent message)
    {
        var dateTime = DateTimeOffset.Now;

        await using (var repository = context.GetRepository<Domain.Entities.Comment>())
        {
            var specification = new FindRootCommentsForPost(message.Key, -1, 0);
            var comments = await repository.QueryAsync(specification, CancellationToken.None);

            if (0 < comments.Length)
            {
                logger.LogWarning($"No comments for post: {message.Key:D}");
                return;
            }

            for (var index = 0; index < comments.Length; index++)
            {
                comments[index].DeletedAt = dateTime;
                logger.LogDebug($"Comment id: {comments[index].Id} for post: {message.Key:D} was deleted");
            }

            await repository.SaveChangesAsync(CancellationToken.None);
        }
    }
}