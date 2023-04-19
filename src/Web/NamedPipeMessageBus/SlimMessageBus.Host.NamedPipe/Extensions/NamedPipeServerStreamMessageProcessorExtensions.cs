using Microsoft.Extensions.Logging;
using SlimMessageBus.Host.NamedPipe.Core;

namespace SlimMessageBus.Host.NamedPipe.Extensions;

internal static class NamedPipeServerStreamMessageProcessorExtensions
{
    public static ILogger<NamedPipeServerStreamMessageProcessor> LogExecuteStarting(
        this ILogger<NamedPipeServerStreamMessageProcessor> logger, int number, string? path, string pipeName)
    {
        logger.LogInformation($"Starting stream instance: {number} for path: \"{path}\" in pipe: \"{pipeName}\"");
        return logger;
    }
    
    public static ILogger<NamedPipeServerStreamMessageProcessor> LogExecuteStarted(
        this ILogger<NamedPipeServerStreamMessageProcessor> logger, int number)
    {
        logger.LogInformation($"Stream instance: {number} started");
        return logger;
    }
    
    public static ILogger<NamedPipeServerStreamMessageProcessor> LogServerStreamConnected(
        this ILogger<NamedPipeServerStreamMessageProcessor> logger, int number, string pipeName)
    {
        logger.LogInformation($"Server stream instance: {number} connected");
        return logger;
    }
    
    public static ILogger<NamedPipeServerStreamMessageProcessor> LogServerStreamDisconnected(
        this ILogger<NamedPipeServerStreamMessageProcessor> logger, int number)
    {
        logger.LogInformation($"Server stream instance: {number} disconnected");
        return logger;
    }
    
    public static ILogger<NamedPipeServerStreamMessageProcessor> LogMessageSent(
        this ILogger<NamedPipeServerStreamMessageProcessor> logger, int number, Type? messageType, int messageLength)
    {
        logger.LogInformation($"Server stream instance: {number} sent message: {messageType?.Name} with length: {messageLength}");
        return logger;
    }
    
    public static ILogger<NamedPipeServerStreamMessageProcessor> LogServerStreamDone(
        this ILogger<NamedPipeServerStreamMessageProcessor> logger, int number)
    {

        return logger;
    }

    public static ILogger<NamedPipeServerStreamMessageProcessor> LogFinalizing(
        this ILogger<NamedPipeServerStreamMessageProcessor> logger, int number)
    {

        return logger;
    }

    public static ILogger<NamedPipeServerStreamMessageProcessor> LogException(
        this ILogger<NamedPipeServerStreamMessageProcessor> logger, int number, Exception exception)
    {

        return logger;
    }
}