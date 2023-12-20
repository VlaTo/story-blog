using MediatR;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Application.Extensions;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Identity.Permission;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Application.Extensions;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.EditPost;

public sealed class EditPostHandler : PostHandlerBase, IRequestHandler<EditPostCommand, Result>
{
    private readonly IAsyncUnitOfWork context;

    public EditPostHandler(
        IAsyncUnitOfWork context,
        ILogger<EditPostHandler> logger)
        : base(logger)
    {
        this.context = context;
    }

    public async Task<Result> Handle(EditPostCommand request, CancellationToken cancellationToken)
    {
        var authenticated = request.CurrentUser.IsAuthenticated();
        string? author;

        if (authenticated)
        {
            if (false == request.CurrentUser.HasPermission(Permissions.Blogs.Update))
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

        await using (var repository = context.GetRepository<Post>())
        {
            var post = await repository.FindPostBySlugOrKeyAsync(request.SlugOrKey, cancellationToken: cancellationToken);

            if (null != post)
            {
                post.Title = request.Details.Title;
                post.Slug.Text = request.Details.Slug;

                await repository.SaveChangesAsync(cancellationToken);

                return Result.Success;
            }
        }

        return new Exception();
    }
}