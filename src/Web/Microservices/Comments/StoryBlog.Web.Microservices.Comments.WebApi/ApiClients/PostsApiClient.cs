using StoryBlog.Web.Microservices.Comments.Application.Services;
using StoryBlog.Web.Microservices.Posts.Shared.Models;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;

namespace StoryBlog.Web.Microservices.Comments.WebApi.ApiClients;

internal sealed class PostsApiClient(HttpClient httpClient) : IPostsApiClient, IDisposable
{
    private bool disposed = false;

    public async Task<PostModel> GetPostAsync(Guid postKey, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"http://localhost:5033/s2s/v1.0-alpha/Post/{postKey:D}"
        );

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

        using (var response = await httpClient.SendAsync(request, cancellationToken))
        {
            var message = response.EnsureSuccessStatusCode();

            await using (var stream = await message.Content.ReadAsStreamAsync(cancellationToken))
            {
                var postModel = JsonSerializer.Deserialize<PostModel>(stream);

                if (null != postModel)
                {
                    return postModel;
                }

                throw new Exception();
            }
        }
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        Dispose(true);
    }

    private void Dispose(bool dispose)
    {
        try
        {
            if (dispose)
            {
                httpClient.Dispose();
            }
        }
        finally
        {
            disposed = true;
        }
    }
}