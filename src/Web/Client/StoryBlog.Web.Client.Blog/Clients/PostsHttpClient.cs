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
using StoryBlog.Web.Client.Blog.Configuration;
using StoryBlog.Web.Client.Blog.Extensions;
using StoryBlog.Web.Client.Blog.Models;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Shared.Models;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using CreatedPostModel = StoryBlog.Web.Microservices.Posts.Shared.Models.CreatedPostModel;

namespace StoryBlog.Web.Client.Blog.Clients;

internal sealed class PostsHttpClient : HttpClientBase
{
    private readonly HttpClientOptions options;

    public PostsHttpClient(
        HttpClient httpClient,
        IOptions<HttpClientOptions> options)
        : base(httpClient)
    {
        this.options = options.Value;
    }

    public async Task<Result<EmptyList, ListAllResponse>> GetPostsAsync(int pageNumber, int pageSize)
    {
        try
        {
            var uri = QueryHelpers.AddQueryString(
                options.Endpoints.Posts.BasePath,
                new Dictionary<string, string?>
                {
                    { "pageNumber", pageNumber.ToString() },
                    { "pageSize", pageSize.ToString() }
                }
            );

            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            
            using (var response = await HttpClient.SendAsync(request))
            {
                var message = response.EnsureSuccessStatusCode();

                using (var stream = await message.Content.ReadAsStreamAsync())
                {
                    var list = await JsonSerializer.DeserializeAsync<ListAllResponse>(stream);

                    if (null == list)
                    {
                        return EmptyList.Instance;
                    }

                    return list;
                }
            }
        }
        catch(Exception exception)
        {
            return exception;
        }
    }

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
            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

            using (var response = await HttpClient.SendAsync(request))
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

    public async Task<Result<EmptyList, IReadOnlyCollection<BriefModel>>> GetTailPostsAsync(Guid postKey, int count)
    {
        try
        {
            var basePath = new Uri(options.Endpoints.Tail.BasePath, UriKind.Absolute);
            var relativeUri = new Uri($"{postKey:D}/{count}", UriKind.Relative);

            if (false == Uri.TryCreate(basePath, relativeUri, out var endpoint))
            {
                return new Exception();
            }

            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

            using (var response = await HttpClient.SendAsync(request))
            {
                var message = response.EnsureSuccessStatusCode();

                using (var stream = await message.Content.ReadAsStreamAsync())
                {
                    var list = await JsonSerializer.DeserializeAsync<IReadOnlyCollection<BriefModel>>(stream);

                    if (null == list)
                    {
                        return EmptyList.Instance;
                    }

                    return new Result<EmptyList, IReadOnlyCollection<BriefModel>>(list);
                }
            }
        }
        catch (Exception exception)
        {
            return exception;
        }
    }

    public async Task<Models.CreatedPostModel?> CreatePostAsync(string title, string slug, string content)
    {
        var uri = new Uri(options.Endpoints.Posts.BasePath, UriKind.Absolute);
        var createPostRequest = new CreatePostRequest
        {
            Title = title,
            Slug = slug,
            Content = content
        };

        var request = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = JsonContent.Create(createPostRequest)
        };

        try
        {
            using (var response = await HttpClient.SendAsync(request))
            {
                var message = response.EnsureSuccessStatusCode();

                using (var stream = await message.Content.ReadAsStreamAsync())
                {
                    var createdPost = await JsonSerializer.DeserializeAsync<CreatedPostModel>(stream);

                    return new Models.CreatedPostModel
                    {
                        Title = createdPost.Title,
                        Slug = createdPost.Slug,
                        Status = createdPost.PublicationStatus,
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

    public async Task<Result> DeletePostAsync(string slugOrKey)
    {
        var basePath = new Uri(options.Endpoints.Post.BasePath, UriKind.Absolute);
        var relativeUri = new Uri(slugOrKey, UriKind.Relative);

        if (false == Uri.TryCreate(basePath, relativeUri, out var endpoint))
        {
            return new Exception("Failed to create Uri");
        }

        var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);

        try
        {
            using (var response = await HttpClient.SendAsync(request))
            {
                var message = response.EnsureSuccessStatusCode();

                using (var stream = await message.Content.ReadAsStreamAsync())
                {
                    var postDeleted = await JsonSerializer.DeserializeAsync<PostDeletedResponse>(stream);
                    return Result.Success;
                }
            }
        }
        catch (Exception exception)
        {
            return exception;
        }
    }

    public async Task<Result> ChangePostVisibilityAsync(Guid postKey, PostVisibilityStatus visibilityStatus)
    {
        var basePath = new Uri(options.Endpoints.Toggle.BasePath, UriKind.Absolute);
        var relativeUri = new Uri(postKey.ToString("N"), UriKind.Relative);

        if (false == Uri.TryCreate(basePath, relativeUri, out var endpoint))
        {
            return new Exception("Failed to create Uri");
        }

        var request = new HttpRequestMessage(HttpMethod.Put, endpoint)
        {
            Content = JsonContent.Create(new PostPublicityRequest
            {
                VisibilityStatus = visibilityStatus
            })
        };

        try
        {
            using (var response = await HttpClient.SendAsync(request))
            {
                var message = response.EnsureSuccessStatusCode();

                /*using (var stream = await message.Content.ReadAsStreamAsync())
                {
                    var temp = await JsonSerializer.DeserializeAsync<PostPublicityResponse>(stream);
                    return Result.Success;
                }*/

                return Result.Success;
            }
        }
        catch (Exception exception)
        {
            return exception;
        }
    }

    public async Task<GeneratedSlugModel?> GenerateSlugAsync(string title)
    {
        try
        {
            var uri = QueryHelpers.AddQueryString(
                options.Endpoints.Slugs.BasePath,
                new Dictionary<string, string?>
                {
                    { "title", title }
                }
            );

            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            using (var response = await HttpClient.SendAsync(request))
            {
                var message = response.EnsureSuccessStatusCode();

                using (var stream = await message.Content.ReadAsStreamAsync())
                {
                    return await JsonSerializer.DeserializeAsync<GeneratedSlugModel>(stream);
                }
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine(exception);
            return null;
        }
    }

    public async Task<PostReferenceModel?> FetchPostReferenceAsync(string slug)
    {
        try
        {
            var basePath = new Uri(options.Endpoints.Slug.BasePath);
            var relativeUri = new Uri(slug, UriKind.Relative);

            if (false == Uri.TryCreate(basePath, relativeUri, out var endpoint))
            {
                return null;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

            using (var response = await HttpClient.SendAsync(request))
            {
                var message = response.EnsureSuccessStatusCode();

                using (var stream = await message.Content.ReadAsStreamAsync())
                {
                    return await JsonSerializer.DeserializeAsync<PostReferenceModel>(stream);
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