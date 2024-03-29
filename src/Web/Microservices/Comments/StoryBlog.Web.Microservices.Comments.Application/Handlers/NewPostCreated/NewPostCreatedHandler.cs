using MediatR;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Microservices.Comments.Domain.Entities;
using StoryBlog.Web.Microservices.Comments.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.NewPostCreated;

internal sealed class NewPostCreatedHandler : HandlerBase, INotificationHandler<NewPostCreatedCommand>
{
    private readonly IAsyncUnitOfWork context;
    private readonly ILogger<NewPostCreatedHandler> logger;

    public NewPostCreatedHandler(
        IAsyncUnitOfWork context,
        ILogger<NewPostCreatedHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task Handle(NewPostCreatedCommand request, CancellationToken cancellationToken)
    {
        await using (var repository = context.GetRepository<Comment>())
        {
            var specification = new RootCommentsForPostSpecification(request.PostKey);
            var exists = await repository.ExistsAsync(specification);

            if (exists)
            {
                //logger.LogCommentsForPostAlreadyExists(message.PostKey);
            }
        }
    }
}