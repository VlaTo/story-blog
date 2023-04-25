using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Client.Blog.Clients.Interfaces;
using StoryBlog.Web.Client.Blog.Configuration;
using StoryBlog.Web.Microservices.Comments.Shared.Models;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using StoryBlog.Web.Client.Blog.Models;

namespace StoryBlog.Web.Client.Blog.Clients;

internal sealed class CommentsHttpClient : HttpClientBase, ICommentsClient
{
    private readonly HttpClientOptions options;

    public CommentsHttpClient(HttpClient httpClient, IOptions<HttpClientOptions> options)
        : base(httpClient)
    {
        this.options = options.Value;
    }

    public async Task<ListAllResponse?> GetCommentsAsync(Guid postKey)
    {
        var basePath = new Uri(options.Endpoints.Comments.BasePath, UriKind.Absolute);
        var relativeUri = new Uri(postKey.ToString("D"), UriKind.Relative);

        if (false == Uri.TryCreate(basePath, relativeUri, out var endpoint))
        {
            return null;
        }

        var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

        try
        {
            using (var response = await Client.SendAsync(request))
            {
                var message = response.EnsureSuccessStatusCode();

                using (var stream = await message.Content.ReadAsStreamAsync())
                {
                    return await JsonSerializer.DeserializeAsync<ListAllResponse>(stream);
                }
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine(exception);
            return null;
        }
    }

    public async Task<CommentModel?> GetCommentAsync(Guid commentKey)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, options.Endpoints.Comments.BasePath);

        try
        {
            using (var response = await Client.SendAsync(request))
            {
                var message = response.EnsureSuccessStatusCode();

                using (var stream = await message.Content.ReadAsStreamAsync())
                {
                    return await JsonSerializer.DeserializeAsync<CommentModel>(stream);
                }
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine(exception);
            return null;
        }
    }

    public async Task<Models.CreatedCommentModel?> CreateCommentAsync(Guid postKey, Guid? parentKey, string text)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, options.Endpoints.Comments.BasePath);

        /*var uri = QueryHelpers.AddQueryString(
            options.Endpoints.Slugs.BasePath,
            new Dictionary<string, string>
            {
                { "title", title }
            }
        );*/

        request.Content = JsonContent.Create(new CreateCommentRequest
        {
            PostKey = postKey,
            ParentKey = parentKey,
            Text = text
        });

        try
        {
            using (var response = await Client.SendAsync(request))
            {
                var message = response.EnsureSuccessStatusCode();

                using (var stream = await message.Content.ReadAsStreamAsync())
                {
                    var createdComment = await JsonSerializer.DeserializeAsync<CommentModel>(stream);

                    if (null != createdComment)
                    {
                        return new Models.CreatedCommentModel(
                            createdComment.Key,
                            createdComment.PostKey,
                            createdComment.ParentKey,
                            createdComment.Text,
                            createdComment.Status,
                            createdComment.CreatedAt
                        );
                    }
                }
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine(exception);
        }

        return null;
    }
}