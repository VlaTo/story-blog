using Microsoft.Extensions.Logging;
using SlimMessageBus.Host.NamedPipe.Core;

namespace SlimMessageBus.Host.NamedPipe.Extensions;

internal static class NamedPipeClientStreamMessageProcessorExtensions
{
    public static ILogger<NamedPipeClientStreamMessageProcessor> LogExecuteStarting(
        this ILogger<NamedPipeClientStreamMessageProcessor> logger, string? path, string pipeName)
    {
        logger.LogInformation($"Starting client stream for path: \"{path}\" in pipe: \"{pipeName}\"");
        return logger;
    }

    public static ILogger<NamedPipeClientStreamMessageProcessor> LogClientStreamConnected(
        this ILogger<NamedPipeClientStreamMessageProcessor> logger)
    {
        logger.LogInformation("Client stream connected");
        return logger;
    }
}