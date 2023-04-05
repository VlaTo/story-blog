using MediatR;
using System.Security.Claims;
using StoryBlog.Web.Microservices.Posts.Application.Models;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.EditPost;

public class EditPostCommand : IRequest<bool>
{
    public Guid Key
    {
        get;
    }

    public EditPostDetails Details
    {
        get;
    }

    public ClaimsPrincipal CurrentUser
    {
        get;
    }

    public EditPostCommand(Guid key, EditPostDetails details, ClaimsPrincipal currentUser)
    {
        Key = key;
        Details = details;
        CurrentUser = currentUser;
    }
}