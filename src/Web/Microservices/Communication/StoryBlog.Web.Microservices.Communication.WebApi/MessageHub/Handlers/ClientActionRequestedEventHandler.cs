using StoryBlog.Web.MessageHub.Messages;
using StoryBlog.Web.Microservices.Communication.MessageHub.Messages;

namespace StoryBlog.Web.Microservices.Communication.WebApi.MessageHub.Handlers;

internal sealed class ClientActionRequestedEventHandler : IHubMessageHandler<ClientActionRequestedHubMessage>
{
    public ClientActionRequestedEventHandler()
    {
    }

    public Task HandleAsync(ClientActionRequestedHubMessage message, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}