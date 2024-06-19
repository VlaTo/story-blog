using MediatR;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Microservices.Comments.Domain.Entities;
using StoryBlog.Web.Microservices.Comments.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.PostPublished;

internal sealed class PostPublishedHandler : INotificationHandler<PostPublishedCommand>
{
    private readonly IAsyncUnitOfWork context;
    private readonly ILogger<PostPublishedHandler> logger;

    public PostPublishedHandler(
        IAsyncUnitOfWork context,
        ILogger<PostPublishedHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task Handle(PostPublishedCommand notification, CancellationToken cancellationToken)
    {
        await using (var repository = context.GetRepository<Comment>())
        {
            var specification = new RootCommentsForPostSpecification(notification.PostKey);
            var exists = await repository.ExistsAsync(specification, cancellationToken);

            if (exists)
            {
                //logger.LogCommentsForPostAlreadyExists(message.PostKey);
            }
        }
    }
}