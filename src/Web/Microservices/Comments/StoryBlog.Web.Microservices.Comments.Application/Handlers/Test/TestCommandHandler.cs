using MediatR;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Comments.Application.Services;
using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.Test;

public class TestCommandHandler : HandlerBase, IRequestHandler<TestCommand, Result<PostModel>>
{
    private readonly IPostsApiClient postsApiClient;

    public TestCommandHandler(IPostsApiClient postsApiClient)
    {
        this.postsApiClient = postsApiClient;
    }

    public async Task<Result<PostModel>> Handle(TestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var post = await postsApiClient.GetPostAsync(request.PostKey, cancellationToken);
            return post;
        }
        catch (Exception exception)
        {
            return exception;
        }
    }
}