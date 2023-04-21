using MediatR;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.GetComments;

public sealed class GetCommentsQuery : IRequest<GetCommentsResult>
{
    public Guid PostKey
    {
        get;
    }

    public int PageNumber
    {
        get;
    }

    public int PageSize
    {
        get;
    }

    public GetCommentsQuery(Guid postKey, int pageNumber, int pageSize, bool includeAll = false)
    {
        PostKey = postKey;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}