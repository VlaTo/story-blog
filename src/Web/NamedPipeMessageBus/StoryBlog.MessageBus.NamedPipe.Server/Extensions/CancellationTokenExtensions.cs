namespace StoryBlog.MessageBus.NamedPipe.Server.Extensions;

internal static class CancellationTokenExtensions
{
    public static async ValueTask WaitAsync(this CancellationToken cancellationToken, TimeSpan? timeout = null)
    {
        var tcs = new TaskCompletionSource();

        using (cancellationToken.Register(tcs.SetResult))
        {
            var timeSpan = timeout.GetValueOrDefault(Timeout.InfiniteTimeSpan);
            await Task.WhenAny(tcs.Task, Task.Delay(timeSpan, cancellationToken));
        }
    }
}