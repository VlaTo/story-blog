using MediatR;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.GetComments;

public sealed class GetCommentsQuery : IRequest<GetCommentsResult>
{
    public Guid PostKey
    {
        get;
    }

    public Guid? ParentKey
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

    public GetCommentsQuery(Guid postKey, Guid? parentKey, int pageNumber, int pageSize, bool includeAll = false)
    {
        PostKey = postKey;
        ParentKey = parentKey;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}