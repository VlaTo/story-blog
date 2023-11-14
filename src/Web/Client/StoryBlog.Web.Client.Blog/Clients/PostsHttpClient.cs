#region MIT Licence
// Copyright 2023 Vladimir Tolmachev
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the “Software”), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies
// or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
#endregion

using Microsoft.Extensions.Options;
using StoryBlog.Web.Client.Blog.Clients.Interfaces;
using StoryBlog.Web.Client.Blog.Configuration;
using StoryBlog.Web.Client.Blog.Models;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Shared.Models;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using CreatedPostModel = StoryBlog.Web.Microservices.Posts.Shared.Models.CreatedPostModel;

namespace StoryBlog.Web.Client.Blog.Clients;

internal sealed class PostsHttpClient : IPostsClient
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly HttpClientOptions options;

    public PostsHttpClient(
        IHttpClientFactory httpClientFactory,
        IOptions<HttpClientOptions> options)
    {
        this.httpClientFactory = httpClientFactory;
        this.options = options.Value;
    }

    /// <inheritdoc cref="IPostsClient.GetPostsAsync" />
    public async Task<Result<EmptyPostsResponse, ListAllResponse>> GetPostsAsync(int pageNumber, int pageSize)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient("PostsApi");
            var request = new HttpRequestMessage(HttpMethod.Get, options.Endpoints.Posts.BasePath);
            
            using (var response = await httpClient.SendAsync(request))
            {
                var message = response.EnsureSuccessStatusCode();

                using (var stream = await message.Content.ReadAsStreamAsync())
                {
                    var model = await JsonSerializer.DeserializeAsync<ListAllResponse>(stream);

                    if (null == model)
                    {
                        return new EmptyPostsResponse();
                    }

                    return model;
                }
            }
        }
        catch(Exception exception)
        {
            return exception;
        }
    }

    /// <inheritdoc cref="IPostsClient.GetPostAsync" />
    public async Task<PostModel?> GetPostAsync(string slugOrKey)
    {
        var basePath = new Uri(options.Endpoints.Post.BasePath, UriKind.Absolute);
        var relativeUri = new Uri(slugOrKey, UriKind.Relative);

        if (false == Uri.TryCreate(basePath, relativeUri, out var endpoint))
        {
            return null;
        }

        try
        {
            var httpClient = httpClientFactory.CreateClient("PostsApi");
            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

            using (var response = await httpClient.SendAsync(request))
            {
                var message = response.EnsureSuccessStatusCode();

                using (var stream = await message.Content.ReadAsStreamAsync())
                {
                    return await JsonSerializer.DeserializeAsync<PostModel>(stream);
                }
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine(exception);
            return null;
        }
    }

    /// <inheritdoc cref="IPostsClient.CreatePostAsync" />
    public async Task<Models.CreatedPostModel?> CreatePostAsync(string title, string slug)
    {
        var uri = new Uri(options.Endpoints.Posts.BasePath, UriKind.Absolute);
        var request = new HttpRequestMessage(HttpMethod.Post, uri);

        request.Content = JsonContent.Create(new CreatePostRequest
        {
            Title = title,
            Slug = slug
        });

        try
        {
            var httpClient = httpClientFactory.CreateClient("PostsApi");

            using (var response = await httpClient.SendAsync(request))
            {
                var message = response.EnsureSuccessStatusCode();

                using (var stream = await message.Content.ReadAsStreamAsync())
                {
                    var createdPost = await JsonSerializer.DeserializeAsync<CreatedPostModel>(stream);

                    return new Models.CreatedPostModel
                    {
                        Title = createdPost.Title,
                        Slug = createdPost.Slug,
                        Status = createdPost.Status,
                        CreatedAt = createdPost.CreatedAt,
                        Location = message.Headers.Location
                    };
                }
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine(exception);
            return null;
        }
    }
}