namespace StoryBlog.Web.Client.Blog.Core;

public sealed class DelegateCommand : DelegateCommandBase
{
    private readonly Action executeMethod;
    private readonly Func<bool> canExecuteMethod;

    public DelegateCommand(Action executeMethod, Func<bool>? canExecuteMethod = null)
    {
        this.executeMethod = executeMethod;
        this.canExecuteMethod = canExecuteMethod ?? Stub.True;
    }

    public bool CanExecute()
    {
        var handler = canExecuteMethod;
        return handler.Invoke();
    }

    public void Execute()
    {
        var handler = executeMethod;
        handler.Invoke();
    }

    protected override bool CanExecute(object? parameter) => CanExecute();

    protected override void Execute(object? parameter) => Execute();
}

public sealed class DelegateCommand<T> : DelegateCommandBase
{
    private readonly Action<T> executeMethod;
    private readonly Func<T, bool> canExecuteMethod;

    public DelegateCommand(Action<T> executeMethod, Func<T, bool>? canExecuteMethod = null)
    {
        this.executeMethod = executeMethod;
        this.canExecuteMethod = canExecuteMethod ?? Stub<T>.True;
    }
    public bool CanExecute(T parameter)
    {
        var handler = canExecuteMethod;
        return handler.Invoke(parameter);
    }

    public void Execute(T parameter)
    {
        var handler = executeMethod;
        handler.Invoke(parameter);
    }

    protected override bool CanExecute(object? parameter) => CanExecute((T)parameter!);

    protected override void Execute(object? parameter) => Execute((T)parameter!);
}