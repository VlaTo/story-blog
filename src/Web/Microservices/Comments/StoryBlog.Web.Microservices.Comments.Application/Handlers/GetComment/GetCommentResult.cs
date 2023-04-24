using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Microservices.Comments.Application.Models;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.GetComment;

public sealed class GetCommentResult : AbstractResult
{
    public Comment? Comment
    {
        get;
    }

    public GetCommentResult(Comment? comment)
    {
        Comment = comment;
    }

    public override bool IsSuccess() => null != Comment;
}