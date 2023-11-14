using MediatR;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Result;
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
        await using (var repository = context.GetRepository<Post>())
        {
            var entity = await FindPostAsync(repository, request.SlugOrKey, cancellationToken);

            if (null != entity)
            {
                entity.Title = request.Details.Title;
                entity.Slug.Text = request.Details.Slug;

                await context.CommitAsync(cancellationToken);

                return Result.Success;
            }
        }

        return new Exception();
    }
}