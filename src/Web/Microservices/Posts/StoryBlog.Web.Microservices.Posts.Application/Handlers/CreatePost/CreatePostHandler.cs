using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Application.Extensions;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Domain.Entities;
using StoryBlog.Web.Common.Identity.Permission;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Application.Configuration;
using StoryBlog.Web.Microservices.Posts.Application.Extensions;
using StoryBlog.Web.Microservices.Posts.Application.Models;
using StoryBlog.Web.Microservices.Posts.Application.Services;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;
using Post = StoryBlog.Web.Microservices.Posts.Domain.Entities.Post;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.CreatePost;

public sealed class CreatePostHandler : HandlerBase, MediatR.IRequestHandler<CreatePostCommand, Result<Guid>>
{
    private readonly IAsyncUnitOfWork context;
    private readonly IMessageBusNotification notification;
    private readonly IBlogPostProcessingManager postProcessingManager;
    private readonly PostsCreateOptions options;
    private readonly ILogger<CreatePostHandler> logger;

    public CreatePostHandler(
        IAsyncUnitOfWork context,
        IMessageBusNotification notification,
        IBlogPostProcessingManager postProcessingManager,
        IOptions<PostsCreateOptions> options,
        ILogger<CreatePostHandler> logger)
    {
        this.context = context;
        this.notification = notification;
        this.postProcessingManager = postProcessingManager;
        this.options = options.Value;
        this.logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var authenticated = request.CurrentUser.IsAuthenticated();
        string? author;

        if (authenticated)
        {
            if (false == request.CurrentUser.HasPermission(Permissions.Blogs.Create))
            {
                return new Exception("Insufficient permissions");
            }

            author = request.CurrentUser.GetSubject();

            if (String.IsNullOrEmpty(author))
            {
                return new Exception("No user identity");
            }
        }
        else
        {
            return new Exception("User not authenticated");
        }

        var post = new Post
        {
            Title = request.Details.Title,
            PublicationStatus = options.ApprovePostWhenCreated ? PublicationStatus.Approved : PublicationStatus.Pending,
            AuthorId = author
        };
        
        post.Slug = new Slug
        {
            Post = post,
            Text = request.Details.Slug
        };
        post.CommentsCounter = new CommentsCounter
        {
            Post = post,
            Counter = 0
        };
        post.Content = new Content
        {
            Post = post,
            Text = request.Details.Text
        };

        logger.LogEntityCreated();

        await using (var repository = context.GetRepository<Post>())
        {
            await repository.AddAsync(post, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
        }

        var newPostCreated = new NewPostCreated(post.Id, post.Key, post.Slug.Text, post.CreateAt, post.AuthorId);

        await Task.WhenAll(
            postProcessingManager.QueuePostProcessingTaskAsync(post.Key, cancellationToken),
            notification.NewPostCreatedAsync(newPostCreated, cancellationToken)
        );

        return post.Key;
    }

    /*private Task NotifyAsync(Post post, CancellationToken cancellationToken)
    {
        var tasks = new List<Task>
        {
            postProcessingManager.QueuePostProcessingTaskAsync(post.Key, cancellationToken)
        };

        if (options.PublishCreatedEvent)
        {
            var createdEvent = new NewPostCreatedEvent(post.Key, post.CreateAt, post.AuthorId);
            tasks.Add(messageBus.Publish(createdEvent, cancellationToken: cancellationToken));
        }

        if (!String.IsNullOrEmpty(options.HubChannelName))
        {
            var publishedMessage = new NewPostPublishedMessage(post.Key, post.Slug.Text);
            tasks.Add(messageHub.SendAsync(options.HubChannelName "post.created", publishedMessage, cancellationToken));
        }

        return Task.WhenAll(tasks);
    }*/
}