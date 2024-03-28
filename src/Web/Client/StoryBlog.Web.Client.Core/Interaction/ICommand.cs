namespace StoryBlog.Web.Client.Core.Interaction;

public interface ICommand<T>
{
    event EventHandler<T> CanExecuteChanged;

    bool CanExecute(T parameter);

    void Execute(T parameter);
}