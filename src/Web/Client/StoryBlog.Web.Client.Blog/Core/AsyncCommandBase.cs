using System.Windows.Input;

namespace StoryBlog.Web.Client.Blog.Core;

public abstract class AsyncCommandBase : DelegateCommandBase, IAsyncCommand
{
    protected AsyncCommandBase()
    {
    }

    Task IAsyncCommand.ExecuteAsync(object? parameter) => ExecuteAsync(parameter);

    protected abstract Task ExecuteAsync(object? parameter);
}