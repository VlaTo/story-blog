using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.MessageHub.Services;
using System.Net.WebSockets;
using StoryBlog.Web.MessageHub.Messages;
using MessageHubOptions = StoryBlog.Web.MessageHub.Server.Configuration.MessageHubOptions;

namespace StoryBlog.Web.MessageHub.Server.Services;

internal sealed class MessageHubHandler : WebSocketTransport
{
    private readonly IServiceProvider serviceProvider;
    private readonly MessageHubService hubService;
    private readonly MessageHubOptions options;

    public MessageHubHandler(
        MessageHubService hubService,
        IServiceProvider serviceProvider,
        WebSocket webSocket,
        MessageHubOptions options)
    {
        WebSocket = webSocket;

        this.serviceProvider = serviceProvider;
        this.hubService = hubService;
        this.options = options;
    }

    public async Task HandleAsync(CancellationToken cancellationToken = default)
    {
        if (WebSocket is not { State: WebSocketState.Open })
        {
            throw new Exception();
        }

        await ReceiveAsync(HandleMessage, cancellationToken);
    }

    public async Task SendAsync(ArraySegment<byte> message, CancellationToken cancellationToken = default)
    {
        await WebSocket.SendAsync(message, WebSocketMessageType.Binary, true, cancellationToken);
    }

    private async Task HandleMessage(ArraySegment<byte> data)
    {
        var message = Message.From(data);

        if (options.Channels.TryGetValue(message.Channel, out var channel))
        {
            var hubMessage = channel.Messages[0];
            var handlerType = hubMessage.Handlers[0];
            var invokerType = typeof(HandlerInvoker<,>).MakeGenericType(hubMessage.MessageType, handlerType);
            
            var invoker = (HandlerInvoker)ActivatorUtilities.CreateInstance(serviceProvider, invokerType, options.Serializer);

            await invoker.InvokeAsync(message.Payload, CancellationToken.None);
        }
    }

    protected override void DoDispose()
    {
        hubService.RemoveSocketHandler(this);
    }

    /// <summary>
    /// 
    /// </summary>
    private abstract class HandlerInvoker
    {
        protected IServiceProvider ServiceProvider
        {
            get;
        }

        protected HandlerInvoker(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public abstract Task InvokeAsync(ArraySegment<byte> payload, CancellationToken cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="THandler"></typeparam>
    private sealed class HandlerInvoker<TMessage, THandler> : HandlerInvoker
        where TMessage : IHubMessage
        where THandler : IHubMessageHandler<TMessage>
    {
        private readonly IHubMessageSerializer serializer;

        public HandlerInvoker(
            IServiceProvider serviceProvider,
            IHubMessageSerializer serializer)
            : base(serviceProvider)
        {
            this.serializer = serializer;
        }

        public override Task InvokeAsync(ArraySegment<byte> payload, CancellationToken cancellationToken)
        {
            var message = serializer.Deserialize(typeof(TMessage), payload);
            return CreateAndInvokeHandlerAsync((TMessage)message, cancellationToken);
        }

        private async Task CreateAndInvokeHandlerAsync(TMessage message, CancellationToken cancellationToken)
        {
            await using (var scope = ServiceProvider.CreateAsyncScope())
            {
                var handler = (IHubMessageHandler<TMessage>)ActivatorUtilities.CreateInstance<THandler>(scope.ServiceProvider);
                await handler.HandleAsync(message, cancellationToken);
            }
        }
    }
}