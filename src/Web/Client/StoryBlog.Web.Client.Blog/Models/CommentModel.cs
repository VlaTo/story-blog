using StoryBlog.Web.Client.Blog.Core;

namespace StoryBlog.Web.Client.Blog.Models;

public class CommentModel : ICommentBase
{
    public Guid Key
    {
        get;
    }

    public Guid PostKey
    {
        get;
    }

    public Guid? ParentKey
    {
        get;
    }

    public string Text
    {
        get;
    }

    public CommentsCollection Comments
    {
        get;
    }

    public DateTime CreateAt
    {
        get;
    }

    public CommentModel(
        Guid key,
        Guid postKey,
        Guid? parentKey,
        string text,
        CommentsCollection comments,
        DateTime createAt)
    {
        Key = key;
        PostKey = postKey;
        ParentKey = parentKey;
        Text = text;
        Comments = comments;
        CreateAt = createAt;
    }
}