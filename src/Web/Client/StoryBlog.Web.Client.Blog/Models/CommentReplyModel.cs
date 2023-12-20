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

    public string? AuthorId
    {
        get;
    }

    public DateTimeOffset CreateAt
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

    public CommentReplyModel(
        Guid? parentKey,
        string correlationKey,
        string? authorId,
        CommentReplyResult result,
        DateTimeOffset createAt)
    {
        ParentKey = parentKey;
        CorrelationKey = correlationKey;
        AuthorId = authorId;
        Result = result;
        CreateAt = createAt;
    }
}