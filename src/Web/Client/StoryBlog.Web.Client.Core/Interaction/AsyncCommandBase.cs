namespace StoryBlog.Web.Client.Core.Interaction;

public abstract class AsyncCommandBase : DelegateCommandBase, IAsyncCommand
{
    Task IAsyncCommand.ExecuteAsync(object? parameter) => ExecuteAsync(parameter);

    protected abstract Task ExecuteAsync(object? parameter);
}