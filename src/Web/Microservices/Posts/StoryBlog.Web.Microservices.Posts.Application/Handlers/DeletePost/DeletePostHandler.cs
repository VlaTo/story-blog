using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Application.Extensions;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Identity.Permission;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Application.Extensions;
using StoryBlog.Web.Microservices.Posts.Application.Services;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.DeletePost;

// ReSharper disable once UnusedMember.Global
public sealed class DeletePostHandler : PostHandlerBase, MediatR.IRequestHandler<DeletePostCommand, Result<Success, NotFound>>
{
    private readonly IAsyncUnitOfWork context;
    private readonly IMessageBusNotification notification;

    public DeletePostHandler(
        IAsyncUnitOfWork context,
        IMessageBusNotification notification,
        ILogger<DeletePostHandler> logger)
        : base(logger)
    {
        this.context = context;
        this.notification = notification;
    }

    public async Task<Result<Success, NotFound>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var authenticated = request.CurrentUser.IsAuthenticated();

        if (authenticated)
        {
            if (false == request.CurrentUser.HasPermission(Permissions.Blogs.Delete))
            {
                return new Exception("Insufficient permissions");
            }

            var author = request.CurrentUser.GetSubject();

            if (String.IsNullOrEmpty(author))
            {
                return new Exception("No user identity");
            }
        }
        else
        {
            return new Exception("User not authenticated");
        }

        try
        {
            await using (var repository = context.GetRepository<Post>())
            {
                var post = await repository.FindPostBySlugOrKeyAsync(request.SlugOrKey, cancellationToken: cancellationToken);

                if (null == post)
                {
                    return NotFound.Instance;
                }

                await repository.RemoveAsync(post, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);

                await notification.PostDeletedAsync(post.Key, post.AuthorId, cancellationToken);

                return Success.Instance;
            }
        }
        catch (Exception exception)
        {
            return exception;
        }
    }

/*    private async Task NotifyAsync(Post post, CancellationToken cancellationToken)
    {
        var tasks = new List<Task>();

        if (options.PublishRemovedEvent)
        {
            var removedEvent = new PostRemovedEvent(post.Key, post.CreateAt, post.AuthorId);
            tasks.Add(notification.Publish(removedEvent, cancellationToken: cancellationToken));
        }

        if (!String.IsNullOrEmpty(options.HubChannelName))
        {
            var removedMessage = new PostRemovedMessage(post.Key, post.Slug.Text);
            tasks.Add(messageHub.SendAsync(options.HubChannelName "post.removed", removedMessage, cancellationToken));
        }

        if (0 < tasks.Count)
        {
            await Task.WhenAll(tasks);
        }
    }*/
}