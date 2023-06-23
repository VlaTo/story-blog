using MediatR;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Comments.Application.Models;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.GetComments;

public sealed class GetCommentsQuery : IRequest<Result<IReadOnlyList<Comment>>>
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