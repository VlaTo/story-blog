using Microsoft.Extensions.Logging;
using SlimMessageBus;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Events;
using StoryBlog.Web.Microservices.Comments.Domain.Entities;
using StoryBlog.Web.Microservices.Comments.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.CreateComment;

public sealed class CreateCommentHandler : HandlerBase, MediatR.IRequestHandler<CreateCommentCommand, Guid?>
{
    private readonly IUnitOfWork context;
    private readonly IMessageBus messageBus;
    private readonly ILogger<CreateCommentHandler> logger;

    public CreateCommentHandler(
        IUnitOfWork context,
        IMessageBus messageBus,
        ILogger<CreateCommentHandler> logger)
    {
        this.context = context;
        this.messageBus = messageBus;
        this.logger = logger;
    }

    public async Task<Guid?> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = new Comment
        {
            PostKey = request.Details.PostKey,
            Text = request.Details.Text,
            Status = CommentStatus.Approved,
            CreateAt = DateTime.UtcNow
        };

        await using (var repository = context.GetRepository<Comment>())
        {
            await repository.AddAsync(comment, cancellationToken);

            if (request.Details.ParentKey.HasValue)
            {
                var specification = new FindParentCommentSpecification(request.Details.PostKey, request.Details.ParentKey.Value, includeChildren: true);
                var parentComment = await repository.FindAsync(specification, cancellationToken);

                if (null == parentComment)
                {
                    throw new Exception();
                }

                parentComment.Comments.Add(comment);
            }

            await repository.SaveChangesAsync(cancellationToken);
        }

        var commentEvent = new BlogCommentEvent(
            comment.Key,
            comment.PostKey,
            comment.Parent?.Key,
            comment.CreateAt,
            BlogCommentAction.Added
        );

        await messageBus.Publish(commentEvent, cancellationToken: cancellationToken);

        return comment.Key;
    }
}