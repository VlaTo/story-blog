using System.Security.Claims;
using MediatR;
using StoryBlog.Web.Common.Application;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPost;

public class GetPostQuery : IRequest<Result<Models.Post>>
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