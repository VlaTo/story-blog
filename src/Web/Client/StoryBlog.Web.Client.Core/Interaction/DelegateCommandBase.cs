using System.Windows.Input;

namespace StoryBlog.Web.Client.Core.Interaction;

public abstract class DelegateCommandBase : ICommand
{
    public virtual event EventHandler? CanExecuteChanged;

    protected DelegateCommandBase()
    {
    }

    bool ICommand.CanExecute(object? parameter) => CanExecute(parameter);

    void ICommand.Execute(object? parameter) => Execute(parameter);

    protected abstract bool CanExecute(object? parameter);

    protected abstract void Execute(object? parameter);

    protected void OnCanExecuteChanged()
    {
        var handler = CanExecuteChanged;

        if (null != handler)
        {
            handler.Invoke(this, EventArgs.Empty);
        }
    }
}