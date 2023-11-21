using Microsoft.Extensions.Logging;
using SlimMessageBus;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Application.Extensions;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Events;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.MessageHub;
using StoryBlog.Web.Microservices.Posts.Application.Extensions;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;
using StoryBlog.Web.Microservices.Posts.Shared.Messages;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.CreatePost;

public sealed class CreatePostHandler : HandlerBase, MediatR.IRequestHandler<CreatePostCommand, Result<Guid>>
{
    private readonly IAsyncUnitOfWork context;
    private readonly IMessageHub messageHub;
    private readonly IMessageBus messageBus;
    private readonly ILogger<CreatePostHandler> logger;

    public CreatePostHandler(
        IAsyncUnitOfWork context,
        IMessageHub messageHub,
        IMessageBus messageBus,
        ILogger<CreatePostHandler> logger)
    {
        this.context = context;
        this.messageHub = messageHub;
        this.messageBus = messageBus;
        this.logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var post = new Post
        {
            Title = request.Details.Title,
            Status = PostStatus.Pending,
            AuthorId = request.CurrentUser.GetSubject() ?? Guid.Empty.ToString("D")
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
            Text = request.Details.Text,
            Brief = request.Details.Brief
        };

        logger.LogEntityCreated();

        await using (var repository = context.GetRepository<Post>())
        {
            await repository.AddAsync(post, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
        }

        var createdEvent = new NewPostCreatedEvent(post.Key, post.CreateAt, post.AuthorId);
        var publishedMessage = new NewPostPublishedMessage(post.Key, post.Slug.Text);

        await Task.WhenAll(
            messageBus.Publish(createdEvent, cancellationToken: cancellationToken),
            messageHub.SendAsync("Test", publishedMessage, cancellationToken)
        );

        return post.Key;
    }
}