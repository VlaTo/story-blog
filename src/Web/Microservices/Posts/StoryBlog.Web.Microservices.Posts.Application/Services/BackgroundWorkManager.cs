using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Application.Models;

namespace StoryBlog.Web.Microservices.Posts.Application.Services;

internal sealed class BackgroundWorkManager : IBackgroundWorkManager
{
    private readonly ILogger<BackgroundWorkManager> logger;
    private readonly Channel<BackgroundTask> channel;
    private readonly ChannelReader<BackgroundTask> channelReader;
    private readonly ChannelWriter<BackgroundTask> channelWriter;

    public BackgroundWorkManager(ILogger<BackgroundWorkManager> logger)
    {
        this.logger = logger;

        channel = Channel.CreateBounded<BackgroundTask>(new BoundedChannelOptions(16)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleWriter = false,
            SingleReader = true
        });

        channelReader = channel.Reader;
        channelWriter = channel.Writer;
    }

    public async ValueTask<IBackgroundTask> QueueNewTaskAsync(double value, CancellationToken cancellationToken)
    {
        var taskId = Guid.NewGuid();
        var timeout = TimeSpan.FromSeconds(value);
        var backgroundTask = new BackgroundTask(taskId, timeout);

        await channelWriter.WriteAsync(backgroundTask, cancellationToken);

        return backgroundTask;
    }

    public async ValueTask<Result<BackgroundTask>> ReadWorkAsync(CancellationToken cancellationToken)
    {
        try
        {
            var backgroundTask = await channelReader.ReadAsync(cancellationToken);

            return new Result<BackgroundTask>(backgroundTask);
        }
        catch (TaskCanceledException exception)
        {
            return new Result<BackgroundTask>(exception);
        }
        catch (Exception exception)
        {
            return new Result<BackgroundTask>(exception);
        }
    }
}