namespace StoryBlog.Web.Microservices.Posts.Application.Services;

internal interface IBlogPostProcessingQueueProvider
{
    IBlogPostProcessingQueue GetQueue();
}