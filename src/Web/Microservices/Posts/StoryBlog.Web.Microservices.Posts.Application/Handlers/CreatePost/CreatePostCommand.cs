using System.Security.Claims;
using MediatR;
using StoryBlog.Web.Microservices.Posts.Application.Models;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.CreatePost;

public sealed class CreatePostCommand : IRequest<Guid?>
{
    public CreatePostDetails Details
    {
        get;
    }

    public ClaimsPrincipal CurrentUser
    {
        get;
    }

    public CreatePostCommand(CreatePostDetails details, ClaimsPrincipal currentUser)
    {
        Details = details;
        CurrentUser = currentUser;
    }
}