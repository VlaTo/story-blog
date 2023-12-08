using Microsoft.Extensions.Logging;
using SlimMessageBus;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Events;
using StoryBlog.Web.Microservices.Comments.Application.Extensions;
using StoryBlog.Web.Microservices.Comments.Domain.Entities;
using StoryBlog.Web.Microservices.Comments.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Comments.Application.MessageBus.Handlers;

public sealed class NewPostCreatedEventHandler : IConsumer<NewPostCreatedEvent>
{
    private readonly IAsyncUnitOfWork context;
    private readonly ILogger<NewPostCreatedEventHandler> logger;

    public NewPostCreatedEventHandler(
        IAsyncUnitOfWork context,
        ILogger<NewPostCreatedEventHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task OnHandle(NewPostCreatedEvent message)
    {
        await using (var repository = context.GetRepository<Comment>())
        {
            var specification = new FindCommentsForPost(message.Key);
            var exists = await repository.ExistsAsync(specification);

            if (exists)
            {
                logger.LogCommentsForPostAlreadyExists(message.Key);
            }
        }
    }
}