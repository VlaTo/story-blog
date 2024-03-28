using Microsoft.Extensions.Logging;
using SlimMessageBus;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Application.Extensions;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Domain.Entities;
using StoryBlog.Web.Common.Events;
using StoryBlog.Web.Common.Identity.Permission;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Comments.Domain.Entities;
using StoryBlog.Web.Microservices.Comments.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.CreateComment;

public sealed class CreateCommentHandler : HandlerBase, MediatR.IRequestHandler<CreateCommentCommand, Result<Guid>>
{
    private readonly IAsyncUnitOfWork context;
    private readonly IMessageBus messageBus;
    private readonly ILogger<CreateCommentHandler> logger;

    public CreateCommentHandler(
        IAsyncUnitOfWork context,
        IMessageBus messageBus,
        ILogger<CreateCommentHandler> logger)
    {
        this.context = context;
        this.messageBus = messageBus;
        this.logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var authenticated = request.CurrentUser.IsAuthenticated();
        string? authorId;

        if (authenticated)
        {
            if (false == request.CurrentUser.HasPermission(Permissions.Comments.Create))
            {
                return new Exception("Insufficient permissions");
            }

            authorId = request.CurrentUser.GetSubject();

            if (String.IsNullOrEmpty(authorId))
            {
                return new Exception("No user identity");
            }
        }
        else
        {
            return new Exception("User not authenticated");
        }

        var comment = new Comment
        {
            PostKey = request.Details.PostKey,
            Text = request.Details.Text,
            PublicationStatus = PublicationStatus.Approved,
            AuthorId = authorId
        };

        int approvedCommentsCount;

        await using (var repository = context.GetRepository<Comment>())
        {
            await repository.AddAsync(comment, cancellationToken);

            if (request.Details.ParentKey.HasValue)
            {
                var specification = new CommentWithParentOrChildrenSpecification(request.Details.PostKey, request.Details.ParentKey.Value, includeChildren: true);
                var parentComment = await repository.FindAsync(specification, cancellationToken);

                if (null == parentComment)
                {
                    return new Exception("No parent");
                }

                parentComment.Comments.Add(comment);
            }

            await repository.SaveChangesAsync(cancellationToken);

            var countSpecification = new ApprovedCommentsForPostSpecification(request.Details.PostKey);
            
            approvedCommentsCount = await repository.CountAsync(countSpecification, cancellationToken);
        }

        var commentEvent = new NewCommentCreatedEvent(
            comment.Key,
            comment.PostKey,
            comment.Parent?.Key,
            authorId,
            approvedCommentsCount
        );

        await messageBus.Publish(commentEvent, cancellationToken: cancellationToken);

        return comment.Key;
    }
}