using Microsoft.Extensions.DependencyInjection;
using SlimMessageBus.Host.NamedPipe.Core;

namespace SlimMessageBus.Host.NamedPipe;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNamedPipeMessageBus(this IServiceCollection services)
    {
        services.AddSingleton<IMessageBufferPool, MemoryMessageBufferPool>();

        return services;
    }
}