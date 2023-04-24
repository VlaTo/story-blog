using System.Security.Claims;
using MediatR;
using StoryBlog.Web.Microservices.Comments.Application.Models;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.CreateComment;

public sealed class CreateCommentCommand : IRequest<Guid?>
{
    public CreateCommentDetails Details
    {
        get;
    }

    public ClaimsPrincipal CurrentUser
    {
        get;
    }

    public CreateCommentCommand(Models.CreateCommentDetails details, ClaimsPrincipal currentUser)
    {
        Details = details;
        CurrentUser = currentUser;
    }
}