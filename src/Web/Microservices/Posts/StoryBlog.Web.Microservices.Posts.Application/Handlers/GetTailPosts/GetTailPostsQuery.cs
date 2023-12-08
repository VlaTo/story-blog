using MediatR;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Application.Models;
using System.Security.Claims;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GetTailPosts;

public sealed class GetTailPostsQuery : IRequest<Result<IReadOnlyList<Brief>>>
{
    public ClaimsPrincipal CurrentUser
    {
        get;
    }

    public Guid TailPostKey
    {
        get;
    }

    public int NumberOfPosts
    {
        get;
    }

    public bool IncludeAll
    {
        get;
    }

    public GetTailPostsQuery(ClaimsPrincipal currentUser, Guid tailPostKey, int numberOfPosts, bool includeAll)
    {
        CurrentUser = currentUser;
        TailPostKey = tailPostKey;
        NumberOfPosts = numberOfPosts;
        IncludeAll = includeAll;
    }
}