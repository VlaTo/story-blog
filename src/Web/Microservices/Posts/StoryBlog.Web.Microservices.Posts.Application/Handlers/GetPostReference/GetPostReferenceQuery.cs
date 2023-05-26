using System.Security.Claims;
using MediatR;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Microservices.Posts.Application.Models;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPostReference;

public class GetPostReferenceQuery : IRequest<Result<PostReference>>
{
    public string Slug
    {
        get;
        set;
    }

    public ClaimsPrincipal CurrentUserPrincipal
    {
        get;
        set;
    }

    public GetPostReferenceQuery(string slug, ClaimsPrincipal currentUserPrincipal)
    {
        Slug = slug;
        CurrentUserPrincipal = currentUserPrincipal;
    }
}