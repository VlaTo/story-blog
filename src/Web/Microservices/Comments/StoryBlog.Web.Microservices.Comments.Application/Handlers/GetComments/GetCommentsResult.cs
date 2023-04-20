using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Microservices.Comments.Application.Models;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.GetComments;

public class GetCommentsResult : AbstractResult
{
    public IReadOnlyList<Comment>? Comments
    {
        get;
    }

    public GetCommentsResult(IReadOnlyList<Comment>? comments)
    {
        Comments = comments;
    }

    public override bool IsSuccess()
    {
        return null != Comments;
    }
}