using MediatR;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Comments.Application.Services;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.Test;

public class TestCommandHandler : HandlerBase, IRequestHandler<TestCommand, Result>
{
    private readonly IPostsApiClient postsApiClient;

    public TestCommandHandler(IPostsApiClient postsApiClient)
    {
        this.postsApiClient = postsApiClient;
    }

    public async Task<Result> Handle(TestCommand request, CancellationToken cancellationToken)
    {
        var post = await postsApiClient.GetPostAsync(request.PostKey, cancellationToken);
        return Result.Success;
    }
}