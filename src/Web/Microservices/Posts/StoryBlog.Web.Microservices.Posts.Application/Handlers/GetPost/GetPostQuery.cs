using System.Security.Claims;
using MediatR;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPost;

public class GetPostQuery : IRequest<GetPostResult>
{
    public Guid Key
    {
        get;
    }

    public ClaimsPrincipal CurrentUser
    {
        get;
    }

    public GetPostQuery(Guid key, ClaimsPrincipal currentUser)
    {
        Key = key;
        CurrentUser = currentUser;
    }
}