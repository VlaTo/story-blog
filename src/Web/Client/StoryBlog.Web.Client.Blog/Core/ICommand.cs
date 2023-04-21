namespace StoryBlog.Web.Client.Blog.Core;

public interface ICommand<T>
{
    event EventHandler<T>? CanExecuteChanged;

    bool CanExecute(T? parameter);

    void Execute(T? parameter);
}