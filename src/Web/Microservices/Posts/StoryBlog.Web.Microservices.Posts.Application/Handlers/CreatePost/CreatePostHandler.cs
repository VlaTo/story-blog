using Microsoft.Extensions.Logging;
using SlimMessageBus;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Events;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Application.Extensions;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.CreatePost;

public sealed class CreatePostHandler : HandlerBase, MediatR.IRequestHandler<CreatePostCommand, Result<Guid>>
{
    private readonly IAsyncUnitOfWork context;
    private readonly IMessageBus messageBus;
    private readonly ILogger<CreatePostHandler> logger;

    public CreatePostHandler(
        IAsyncUnitOfWork context,
        IMessageBus messageBus,
        ILogger<CreatePostHandler> logger)
    {
        this.context = context;
        this.messageBus = messageBus;
        this.logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var post = new Post
        {
            Title = request.Details.Title,
            Status = PostStatus.Pending
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

        var message = new BlogPostEvent(post.Key, BlogPostAction.Submitted);

        await messageBus.Publish(message, cancellationToken: cancellationToken);

        return post.Key;
    }
}