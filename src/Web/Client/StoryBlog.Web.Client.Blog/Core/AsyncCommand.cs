namespace StoryBlog.Web.Client.Blog.Core;

public sealed class AsyncCommand : AsyncCommandBase
{
    private readonly Func<object?, Task> executeMethod;
    private readonly Func<object?, bool> canExecuteMethod;

    public bool IsRunning
    {
        get;
        private set;
    }

    public AsyncCommand(Func<object?, Task> executeMethod, Func<object?, bool>? canExecuteMethod = null)
    {
        this.executeMethod = executeMethod;
        this.canExecuteMethod = canExecuteMethod ?? Stub<object?>.True;

        IsRunning = false;
    }

    protected override bool CanExecute(object? parameter)
    {
        var handler = canExecuteMethod;
        return handler.Invoke(parameter);
    }

    protected override void Execute(object? parameter)
    {
        if (IsRunning)
        {
            return;
        }

        IsRunning = true;

        var synchronizationContext = SynchronizationContext.Current ?? new SynchronizationContext();
        var cts = new CancellationTokenSource();

        try
        {
            TaskRunner.Instance.QueueTask(executeMethod.Invoke(parameter), synchronizationContext, OnTaskComplete, cts.Token);
        }
        finally
        {
            IsRunning = false;
        }
    }

    protected override Task ExecuteAsync(object? parameter)
    {

        throw new NotImplementedException();
    }

    private void OnTaskComplete()
    {
        ;
    }
}