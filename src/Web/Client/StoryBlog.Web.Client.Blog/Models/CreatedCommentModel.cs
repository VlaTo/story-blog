using StoryBlog.Web.Microservices.Comments.Shared.Models;

namespace StoryBlog.Web.Client.Blog.Models;

public sealed class CreatedCommentModel
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

    public CommentPublicationStatus Status
    {
        get;
    }

    public DateTime CreatedAt
    {
        get;
    }

    public CreatedCommentModel(
        Guid key,
        Guid postKey,
        Guid? parentKey,
        string text,
        CommentPublicationStatus status,
        DateTime createdAt)
    {
        Key=key;
        PostKey=postKey;
        ParentKey=parentKey;
        Text=text;
        Status=status;
        CreatedAt=createdAt;
    }
}