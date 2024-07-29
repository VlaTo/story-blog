using System.Diagnostics.CodeAnalysis;
using StoryBlog.Web.Identity.Client.Extensions;
using StoryBlog.Web.Microservices.Comments.Application.Services;
using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Microservices.Comments.WebApi.Services;

internal sealed class TestPostsApiClient : IPostsApiClient
{
    private readonly IHttpClientFactory httpClientFactory;

    public TestPostsApiClient(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<PostModel> GetPostAsync(Guid post, CancellationToken cancellationToken)
    {
        using var httpClient = httpClientFactory.CreateClient("test");

        var disco = await httpClient.GetDiscoveryDocumentAsync("http://localhost:5030/", cancellationToken);

        return new PostModel();
    }
}