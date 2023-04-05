using MediatR;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Posts.Application.Extensions;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;
using StoryBlog.Web.Microservices.Posts.Domain.Interfaces;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.CreatePost;

public sealed class CreatePostHandler : IRequestHandler<CreatePostCommand, Guid?>
{
    private readonly IUnitOfWork context;
    private readonly ILogger<CreatePostHandler> logger;

    public CreatePostHandler(IUnitOfWork context, ILogger<CreatePostHandler> logger)
    {
        this.context = context;
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
            await context.CommitAsync(cancellationToken);
        }

        return post.Key;
    }
}