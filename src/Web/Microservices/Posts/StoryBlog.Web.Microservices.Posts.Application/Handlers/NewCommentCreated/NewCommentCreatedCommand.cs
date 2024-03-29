using MediatR;
using StoryBlog.Web.Common.Result;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.NewCommentCreated;

public sealed record NewCommentCreatedCommand(
    Guid Key,
    Guid PostKey,
    Guid? ParentKey,
    int ApprovedCommentsCount
) : IRequest<Result>;