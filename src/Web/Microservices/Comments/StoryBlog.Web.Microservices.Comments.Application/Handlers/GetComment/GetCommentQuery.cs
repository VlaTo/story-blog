using MediatR;
using StoryBlog.Web.Common.Application;
using System.Security.Claims;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.GetComment;

public sealed class GetCommentQuery : IRequest<Result<Models.Comment>>
{
    public Guid Key
    {
        get;
    }

    public ClaimsPrincipal CurrentUser
    {
        get;
    }

    public GetCommentQuery(Guid key, ClaimsPrincipal currentUser)
    {
        Key = key;
        CurrentUser = currentUser;
    }
}