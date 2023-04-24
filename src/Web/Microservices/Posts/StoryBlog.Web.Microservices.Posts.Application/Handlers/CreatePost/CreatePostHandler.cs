using Microsoft.Extensions.Logging;
using SlimMessageBus;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Events;
using StoryBlog.Web.Microservices.Posts.Application.Extensions;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.CreatePost;

public sealed class CreatePostHandler : HandlerBase, MediatR.IRequestHandler<CreatePostCommand, Guid?>
{
    private readonly IUnitOfWork context;
    private readonly IMessageBus messageBus;
    private readonly ILogger<CreatePostHandler> logger;

    public CreatePostHandler(IUnitOfWork context, IMessageBus messageBus, ILogger<CreatePostHandler> logger)
    {
        this.context = context;
        this.messageBus = messageBus;
        this.logger = logger;
    }

    public async Task<Guid?> Handle(CreatePostCommand request, CancellationToken cancellationToken)
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

        logger.LogEntityCreated();

        await using (var repository = context.GetRepository<Post>())
        {
            await repository.AddAsync(post, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
        }

        await messageBus.Publish(new BlogPostEvent(post.Key, BlogPostAction.Submitted), cancellationToken: cancellationToken);

        return post.Key;
    }
}