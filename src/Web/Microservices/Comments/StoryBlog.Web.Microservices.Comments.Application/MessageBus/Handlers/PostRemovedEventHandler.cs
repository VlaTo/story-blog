using Microsoft.Extensions.Logging;
using SlimMessageBus;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Events;
using StoryBlog.Web.Microservices.Comments.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Comments.Application.MessageBus.Handlers;

public sealed class PostRemovedEventHandler : IConsumer<PostRemovedEvent>
{
    private readonly IAsyncUnitOfWork context;
    private readonly ILogger<NewPostCreatedMessageConsumer> logger;

    public PostRemovedEventHandler(
        IAsyncUnitOfWork context,
        ILogger<NewPostCreatedMessageConsumer> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task OnHandle(PostRemovedEvent message)
    {
        await using (var repository = context.GetRepository<Domain.Entities.Comment>())
        {
            var specification = new RootCommentsForPostSpecification(message.Key);
            var commentDeleted = await repository.RemoveAsync(specification, CancellationToken.None);

            logger.LogWarning($"Comments deleted: {commentDeleted} for post: {message.Key:N}");

            await repository.SaveChangesAsync(CancellationToken.None);
        }
    }
}