namespace StoryBlog.Web.Microservices.Posts.Application.Configuration;

public sealed class PostsCreateOptions
{
    public bool ApprovePostWhenCreated
    {
        get; 
        set;
    }

    public bool PublishCreatedEvent
    {
        get;
        set;
    }

    public string HubChannelName
    {
        get;
        set;
    }
}