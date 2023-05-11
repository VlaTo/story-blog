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

    public string CorrelationKey
    {
        get;
    }

    public CommentReplyResult Result
    {
        get;
    }

    public CommentReplyModel(Guid? parentKey, string correlationKey, CommentReplyResult result, DateTime createAt)
    {
        ParentKey = parentKey;
        CorrelationKey = correlationKey;
        Result = result;
        CreateAt = createAt;
    }
}