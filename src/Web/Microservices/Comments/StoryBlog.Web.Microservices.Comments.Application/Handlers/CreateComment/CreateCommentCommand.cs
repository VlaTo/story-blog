using MediatR;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Comments.Application.Models;
using System.Security.Claims;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.CreateComment;

public sealed class CreateCommentCommand : IRequest<Result<Guid>>
{
    public CreateCommentDetails Details
    {
        get;
    }

    public ClaimsPrincipal CurrentUser
    {
        get;
    }

    public CreateCommentCommand(CreateCommentDetails details, ClaimsPrincipal currentUser)
    {
        Details = details;
        CurrentUser = currentUser;
    }
}