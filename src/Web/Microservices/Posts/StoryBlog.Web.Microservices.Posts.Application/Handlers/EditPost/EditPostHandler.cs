using MediatR;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;
using StoryBlog.Web.Microservices.Posts.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.EditPost;

public sealed class EditPostHandler : HandlerBase, IRequestHandler<EditPostCommand, bool>
{
    private readonly IUnitOfWork context;
    private readonly ILogger<EditPostHandler> logger;

    public EditPostHandler(
        IUnitOfWork context,
        ILogger<EditPostHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<bool> Handle(EditPostCommand request, CancellationToken cancellationToken)
    {
        await using (var repository = context.GetRepository<Post>())
        {
            var specification = new FindPostByKeySpecification(request.Key);
            var entity = await repository
                .FindAsync(specification, cancellationToken)
                .ConfigureAwait(false);

            if (null != entity)
            {
                entity.Title = request.Details.Title;
                entity.Slug.Text = request.Details.Slug;

                await context.CommitAsync(cancellationToken);

                return true;
            }
        }

        return false;
    }
}