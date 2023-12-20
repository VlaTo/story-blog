using MediatR;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Application.Extensions;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Identity.Permission;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Application.Extensions;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.TogglePublicity;

internal sealed class TogglePostPublicityHandler : PostHandlerBase, IRequestHandler<TogglePostPublicityCommand, Result>
{
    private readonly IAsyncUnitOfWork context;

    public TogglePostPublicityHandler(
        IAsyncUnitOfWork context,
        ILogger<TogglePostPublicityHandler> logger)
        : base(logger)
    {
        this.context = context;
    }

    public async Task<Result> Handle(TogglePostPublicityCommand request, CancellationToken cancellationToken)
    {
        var authenticated = request.CurrentUser.IsAuthenticated();
        string? userId;

        if (authenticated)
        {
            if (false == request.CurrentUser.HasPermission(Permissions.Blogs.Update))
            {
                return new Exception("Insufficient permissions");
            }

            userId = request.CurrentUser.GetSubject();

            if (String.IsNullOrEmpty(userId))
            {
                return new Exception("No user identity");
            }
        }
        else
        {
            return new Exception("User not authenticated");
        }

        await using (var repository = context.GetRepository<Domain.Entities.Post>())
        {
            var post = await repository.FindPostBySlugOrKeyAsync(request.SlugOrKey, cancellationToken: cancellationToken);

            if (null == post)
            {
                return new Exception("Post not found");
            }

            if (false == String.Equals(post.AuthorId, userId))
            {
                return new Exception("User is not a author of the post specified");
            }

            if (post.VisibilityStatus != request.VisibilityStatus)
            {
                post.VisibilityStatus = request.VisibilityStatus;

                await repository.SaveChangesAsync(cancellationToken);
                
                Logger.LogDebug($"Post: '{request.SlugOrKey}' set {nameof(post.VisibilityStatus)} to: {request.VisibilityStatus}");
            }

            return Result.Success;
        }
    }
}