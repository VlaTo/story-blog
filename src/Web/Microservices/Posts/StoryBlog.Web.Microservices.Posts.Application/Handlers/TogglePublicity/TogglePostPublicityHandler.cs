using MediatR;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Application.Extensions;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Identity.Permission;
using StoryBlog.Web.Common.Result;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.TogglePublicity;

internal sealed class TogglePostPublicityHandler : PostHandlerBase, IRequestHandler<TogglePostPublicityCommand, Result<(Guid PostKey, bool IsPublic)>>
{
    private readonly IAsyncUnitOfWork context;

    public TogglePostPublicityHandler(
        IAsyncUnitOfWork context,
        ILogger<TogglePostPublicityHandler> logger)
        : base(logger)
    {
        this.context = context;
    }

    public async Task<Result<(Guid PostKey, bool IsPublic)>> Handle(TogglePostPublicityCommand request, CancellationToken cancellationToken)
    {
        var authenticated = request.CurrentUser.IsAuthenticated();

        if (authenticated)
        {
            if (false == request.CurrentUser.HasPermission(Permissions.Blogs.View))
            {
                return new Exception("Insufficient permissions");
            }

            var userId = request.CurrentUser.GetSubject();

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
            var post = await FindPostAsync(repository, request.SlugOrKey, cancellationToken);

            if (null != post)
            {
                if (post.IsPublic != request.IsPublic)
                {
                    post.IsPublic = request.IsPublic;
                    await repository.SaveChangesAsync(cancellationToken);
                }

                return (PostKey: post.Key, post.IsPublic);
            }
        }

        return new Exception("Failed to toggle publicity");
    }
}