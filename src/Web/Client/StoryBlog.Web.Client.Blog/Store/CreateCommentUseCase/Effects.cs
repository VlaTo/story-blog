using Fluxor;
using StoryBlog.Web.Client.Blog.Clients.Interfaces;

namespace StoryBlog.Web.Client.Blog.Store.CreateCommentUseCase;

public class CreateCommentActionEffect : Effect<CreateCommentAction>
{
    private readonly ICommentsClient client;

    public CreateCommentActionEffect(ICommentsClient client)
    {
        this.client = client;
    }

    public override async Task HandleAsync(CreateCommentAction action, IDispatcher dispatcher)
    {
        try
        {
            await client.CreateCommentAsync(action.PostKey, action.ParentKey, action.Text);

            //dispatcher.Dispatch(new );
        }
        catch
        {
            ;
        }
    }
}