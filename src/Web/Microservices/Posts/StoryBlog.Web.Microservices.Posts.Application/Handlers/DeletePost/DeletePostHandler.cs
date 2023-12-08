using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SlimMessageBus;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Events;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.MessageHub;
using StoryBlog.Web.Microservices.Posts.Application.Configuration;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;
using StoryBlog.Web.Microservices.Posts.Shared.Messages;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.DeletePost;

public sealed class DeletePostHandler : PostHandlerBase, MediatR.IRequestHandler<DeletePostCommand, Result<Success, NotFound>>
{
    private readonly IAsyncUnitOfWork context;
    private readonly IMessageHub messageHub;
    private readonly IMessageBus messageBus;
    private readonly PostsCreateOptions options;

    public DeletePostHandler(
        IAsyncUnitOfWork context,
        IMessageHub messageHub,
        IMessageBus messageBus,
        IOptionsSnapshot<PostsCreateOptions> options,
        ILogger<DeletePostHandler> logger)
        : base(logger)
    {
        this.context = context;
        this.messageHub = messageHub;
        this.messageBus = messageBus;
        this.options = options.Value;
    }

    public async Task<Result<Success, NotFound>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await using (var repository = context.GetRepository<Post>())
            {
                var post = await FindPostAsync(repository, request.SlugOrKey, cancellationToken);

                if (null == post)
                {
                    return NotFound.Instance;
                }

                await repository.RemoveAsync(post, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);
                await NotifyAsync(post, cancellationToken);

                return Success.Instance;
            }
        }
        catch (Exception exception)
        {
            return exception;
        }
    }

    private async Task NotifyAsync(Post post, CancellationToken cancellationToken)
    {
        var tasks = new List<Task>();

        if (options.PublishRemovedEvent)
        {
            var removedEvent = new PostRemovedEvent(post.Key, post.CreateAt, post.AuthorId);
            tasks.Add(messageBus.Publish(removedEvent, cancellationToken: cancellationToken));
        }

        if (!String.IsNullOrEmpty(options.HubChannelName))
        {
            var removedMessage = new PostRemovedMessage(post.Key, post.Slug.Text);
            tasks.Add(messageHub.SendAsync(/*options.HubChannelName*/"post.removed", removedMessage, cancellationToken));
        }

        if (0 < tasks.Count)
        {
            await Task.WhenAll(tasks);
        }
    }
}