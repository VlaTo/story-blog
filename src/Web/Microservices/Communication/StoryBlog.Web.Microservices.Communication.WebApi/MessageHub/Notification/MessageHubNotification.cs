using AutoMapper;
using StoryBlog.Web.MessageHub;
using StoryBlog.Web.Microservices.Communication.Application.Models;
using StoryBlog.Web.Microservices.Communication.Application.Services;
using StoryBlog.Web.Microservices.Communication.MessageHub.Messages;

namespace StoryBlog.Web.Microservices.Communication.WebApi.MessageHub.Notification;

public class MessageHubNotification : IMessageHubNotification
{
    private const string PostCreatedChannel = "post.created";

    private readonly IMessageHub messageHub;
    private readonly IMapper mapper;

    public MessageHubNotification(IMessageHub messageHub, IMapper mapper)
    {
        this.messageHub = messageHub;
        this.mapper = mapper;
    }

    public Task PublishNewPostCreatedEventAsync(PublishNewPostCreated context, CancellationToken cancellationToken)
    {
        // send message to the client
        var postPublished = mapper.Map<NewPostPublishedHubMessage>(context);
        return messageHub.PublishAsync(PostCreatedChannel, postPublished, cancellationToken);
    }
}