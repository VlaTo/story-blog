using MediatR;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Microservices.Comments.Domain.Entities;
using StoryBlog.Web.Microservices.Comments.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.PostDeleted;

internal sealed class PostDeletedHandler : INotificationHandler<PostDeletedCommand>
{
    private readonly IAsyncUnitOfWork context;
    private readonly ILogger<PostDeletedHandler> logger;

    public PostDeletedHandler(
        IAsyncUnitOfWork context,
        ILogger<PostDeletedHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task Handle(PostDeletedCommand notification, CancellationToken cancellationToken)
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