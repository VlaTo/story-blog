using System.Security.Claims;
using MediatR;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Application.Models;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPosts;

public class GetPostsQuery : IRequest<Result<(IReadOnlyList<Brief> Posts, int PageNumber, int PageSize, int PagesCount)>>
{
    public int PageNumber
    {
        get;
    }

    public int PageSize
    {
        get;
    }

    public ClaimsPrincipal CurrentUser
    {
        get;
    }

    public bool IncludeAll
    {
        get;
    }

    public GetPostsQuery(ClaimsPrincipal currentUser, int pageNumber, int pageSize, bool includeAll = false)
    {
        CurrentUser = currentUser;
        PageNumber = pageNumber;
        PageSize = pageSize;
        IncludeAll = includeAll;
    }
}