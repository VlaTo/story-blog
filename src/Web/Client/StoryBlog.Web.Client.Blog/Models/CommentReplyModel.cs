namespace StoryBlog.Web.Client.Blog.Models;

public enum CommentReplyResult
{
    Failed,
    Publishing
}

public sealed class CommentReplyModel : ICommentBase
{
    public Guid? ParentKey
    {
        get;
    }

    public DateTime CreateAt
    {
        get;
    }

    public string CorrelationId
    {
        get;
    }

    public CommentReplyResult Result
    {
        get;
    }

    public CommentReplyModel(Guid? parentKey, string correlationId, CommentReplyResult result, DateTime createAt)
    {
        ParentKey = parentKey;
        CorrelationId = correlationId;
        Result = result;
        CreateAt = createAt;
    }
}