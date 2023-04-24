using Fluxor;

namespace StoryBlog.Web.Client.Blog.Store.CreateCommentUseCase;

public sealed class CreateCommentReducer : Reducer<CommentState, CreateCommentAction>
{
    public override CommentState Reduce(CommentState state, CreateCommentAction action)
        => new CommentState(StoreState.Loading, state.Text, state.DateTime);
}