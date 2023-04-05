using MediatR;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPosts;

public class GetPostsQuery : IRequest<GetPostsResult>
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