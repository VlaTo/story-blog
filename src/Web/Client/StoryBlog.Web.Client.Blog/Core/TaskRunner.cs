using StoryBlog.Web.Client.Blog.Extensions;

namespace StoryBlog.Web.Client.Blog.Core;

internal sealed class TaskRunner
{
    public static readonly TaskRunner Instance;

    private readonly object gate;
    private readonly List<NotifyTaskCompletion> collection;
    private readonly List<Task> tasks;

    private TaskRunner()
    {
        gate = new object();
        collection = new List<NotifyTaskCompletion>();
        tasks = new List<Task>();
    }

    static TaskRunner()
    {
        Instance = new TaskRunner();
    }

    public void QueueTask(Task task, SynchronizationContext? synchronizationContext, Action completeCallback, CancellationToken cancellationToken)
    {
        lock (gate)
        {
            var completion = new NotifyTaskCompletion(task, synchronizationContext, completeCallback, cancellationToken);
            
            collection.Add(completion);
            completion.RunAsync().FireAndForget();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private sealed class NotifyTaskCompletion
    {
        private readonly SynchronizationContext synchronizationContext;
        private readonly Action completeCallback;
        private readonly CancellationToken cancellationToken;

        public Task Task
        {
            get;
        }

        public NotifyTaskCompletion(
            Task task,
            SynchronizationContext synchronizationContext,
            Action completeCallback,
            CancellationToken cancellationToken)
        {
            this.synchronizationContext = synchronizationContext;
            this.completeCallback = completeCallback;
            this.cancellationToken = cancellationToken;

            Task = task;
        }

        public async Task RunAsync()
        {
            try
            {
                await Task;
            }
            catch
            {
                ;
            }
            finally
            {
                synchronizationContext.Send(OnTaskComplete, null);
            }
        }

        private void OnTaskComplete(object? state)
        {
            completeCallback.Invoke();
        }
    }
}