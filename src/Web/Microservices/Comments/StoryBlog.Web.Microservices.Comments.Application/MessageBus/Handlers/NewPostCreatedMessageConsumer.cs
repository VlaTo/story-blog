using Microsoft.Extensions.Logging;
using SlimMessageBus;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Microservices.Comments.Application.Extensions;
using StoryBlog.Web.Microservices.Comments.Domain.Entities;
using StoryBlog.Web.Microservices.Comments.Domain.Specifications;
using StoryBlog.Web.Microservices.Posts.Shared.Messages;

namespace StoryBlog.Web.Microservices.Comments.Application.MessageBus.Handlers;

public sealed class NewPostCreatedMessageConsumer : IConsumer<NewPostCreatedMessage>
{
    private readonly IAsyncUnitOfWork context;
    private readonly ILogger<NewPostCreatedMessageConsumer> logger;

    public NewPostCreatedMessageConsumer(
        IAsyncUnitOfWork context,
        ILogger<NewPostCreatedMessageConsumer> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task OnHandle(NewPostCreatedMessage message)
    {
        logger.LogError("StoryBlog.Web.Microservices.Comments.Application.MessageBus.Handlers.NewPostCreatedEventHandler");
        await using (var repository = context.GetRepository<Comment>())
        {
            var specification = new RootCommentsForPostSpecification(message.PostKey);
            var exists = await repository.ExistsAsync(specification);

            if (exists)
            {
                logger.LogCommentsForPostAlreadyExists(message.PostKey);
            }
        }
    }
}