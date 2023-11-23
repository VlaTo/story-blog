using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Result.Extensions;

namespace StoryBlog.Web.Microservices.Posts.Application.Services;

internal sealed class SampleBackgroundService : BackgroundService
{
    private readonly IServiceProvider serviceProvider;
    private readonly BackgroundWorkManager workManager;
    private readonly ILogger<SampleBackgroundService> logger;

    public SampleBackgroundService(
        IServiceProvider serviceProvider,
        BackgroundWorkManager workManager,
        ILogger<SampleBackgroundService> logger)
    {
        this.serviceProvider = serviceProvider;
        this.workManager = workManager;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogDebug("Starting background processing");

        try
        {
            // queue persisted backgroundTask from store
            await workManager.QueueNewTaskAsync(3.0d, stoppingToken);

            // main propessing loop
            while (true)
            {
                logger.LogDebug("Ready to sleep");

                var result = await workManager.ReadWorkAsync(stoppingToken);
                
                if (result.Failed())
                {
                    break;
                }

                logger.LogDebug($"Processing task with ID: {result.Value.Id:D}");

                using (serviceProvider.CreateScope())
                {
                    logger.LogDebug($"Sleep timeout: {result.Value.Timeout:g}");

                    await Task.Delay(result.Value.Timeout, stoppingToken);
                }
            }

            // post processing
            await Task.Delay(TimeSpan.FromSeconds(1.0d), stoppingToken);
        }
        finally
        {
            logger.LogDebug("Background processing completed");
        }
    }
}