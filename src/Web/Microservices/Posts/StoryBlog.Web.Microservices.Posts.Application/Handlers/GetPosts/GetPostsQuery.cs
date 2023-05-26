using MediatR;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Microservices.Posts.Application.Models;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPosts;

public class GetPostsQuery : IRequest<Result<IReadOnlyList<Brief>>>
{
    public int PageNumber
    {
        get;
    }

    public int PageSize
    {
        get;
    }

    public bool IncludeAll
    {
        get;
    }

    public GetPostsQuery(int pageNumber, int pageSize, bool includeAll = false)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        IncludeAll = includeAll;
    }
}