using Fluxor;

namespace StoryBlog.Web.Client.Blog.Store.CreateCommentUseCase;

[FeatureState]
public sealed class CommentState : AbstractStore
{
    public string? Text
    {
        get;
    }

    public DateTime? DateTime
    {
        get;
    }

    public CommentState()
        : base(StoreState.Empty)
    {
    }

    public CommentState(StoreState storeState, string? text, DateTime? dateTime)
        : base(storeState)
    {
        Text = text;
        DateTime = dateTime;
    }
}