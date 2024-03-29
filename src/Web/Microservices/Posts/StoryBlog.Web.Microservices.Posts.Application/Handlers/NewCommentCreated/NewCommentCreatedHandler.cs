using MediatR;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.NewCommentCreated;

internal sealed class NewCommentCreatedHandler : HandlerBase, IRequestHandler<NewCommentCreatedCommand, Result>
{
    private readonly IAsyncUnitOfWork context;
    private readonly ILogger<NewCommentCreatedHandler> logger;

    public NewCommentCreatedHandler(
        IAsyncUnitOfWork context,
        ILogger<NewCommentCreatedHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<Result> Handle(NewCommentCreatedCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"New Comment Created received event for key: \"{request.Key}\"");

        if (false == await UpdateCommentsCountAsync(request.PostKey, request.ApprovedCommentsCount))
        {
            logger.LogWarning($"No post found for key: {request.PostKey}");
        }

        return Result.Success;
    }

    private async Task<bool> UpdateCommentsCountAsync(Guid key, int approvedCommentsCount)
    {
        await using (var repository = context.GetRepository<Domain.Entities.Post>())
        {
            var post = await repository.FindAsync(new FindPostByKeySpecification(key, includeAll: true));

            if (null == post)
            {
                return false;
            }

            post.CommentsCounter.Counter = approvedCommentsCount;

            await repository.SaveChangesAsync();
        }

        return true;
    }
}