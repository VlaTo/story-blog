using System.Windows.Input;

namespace StoryBlog.Web.Client.Core.Interaction;

public interface IAsyncCommand : ICommand
{
    Task ExecuteAsync(object? parameter);
}

public interface IAsyncCommand<T>
{
    event EventHandler<T>? CanExecuteChanged;

    bool CanExecute(T? parameter);

    Task ExecuteAsync(object? parameter);
}