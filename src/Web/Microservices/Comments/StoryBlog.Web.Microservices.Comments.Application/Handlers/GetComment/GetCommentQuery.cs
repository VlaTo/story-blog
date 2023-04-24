using System.Security.Claims;
using MediatR;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.GetComment;

public sealed class GetCommentQuery : IRequest<GetCommentResult>
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