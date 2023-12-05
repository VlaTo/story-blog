using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Posts.Application.Models;
using System.Threading.Channels;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Application.Services;

internal sealed class BlogPostProcessingQueueProvider : IBlogPostProcessingQueueProvider
{
    private const int DefaultCapacity = 16;

    private readonly ILogger<BlogPostProcessingQueueProvider> logger;
    private readonly Channel<BackgroundTask> channel;
    private readonly IBlogPostProcessingQueue queue;

    public BlogPostProcessingQueueProvider(ILogger<BlogPostProcessingQueueProvider> logger)
    {
        this.logger = logger;
        
        channel = Channel.CreateBounded<BackgroundTask>(new BoundedChannelOptions(DefaultCapacity)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleWriter = false,
            SingleReader = true
        });
        queue = new BlogPostProcessingQueue(channel);
    }

    public IBlogPostProcessingQueue GetQueue() => queue;

    #region BlogPostProcessingQueue

    private sealed class BlogPostProcessingQueue : IBlogPostProcessingQueue
    {
        private readonly Channel<BackgroundTask> channel;

        public BlogPostProcessingQueue(Channel<BackgroundTask> channel)
        {
            this.channel = channel;
        }

        public ValueTask EnqueueTaskAsync(IBackgroundTask backgroundTask, CancellationToken cancellationToken)
        {
            if (backgroundTask is BackgroundTask bt)
            {
                return channel.Writer.WriteAsync(bt, cancellationToken);
            }

            return ValueTask.CompletedTask;
        }

        public async ValueTask<Result<IBackgroundTask>> DequeueTaskAsync(CancellationToken cancellationToken)
        {
            try
            {
                var backgroundTask = await channel.Reader.ReadAsync(cancellationToken);

                return new Result<IBackgroundTask>(backgroundTask);
            }
            catch (TaskCanceledException exception)
            {
                return new Result<IBackgroundTask>(exception);
            }
            catch (Exception exception)
            {
                return new Result<IBackgroundTask>(exception);
            }
        }
    }

    #endregion
}