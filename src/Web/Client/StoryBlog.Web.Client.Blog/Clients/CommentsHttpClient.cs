﻿using Microsoft.Extensions.Options;
using StoryBlog.Web.Client.Blog.Clients.Interfaces;
using StoryBlog.Web.Client.Blog.Configuration;
using StoryBlog.Web.Microservices.Comments.Shared.Models;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using StoryBlog.Web.Client.Blog.Extensions;

namespace StoryBlog.Web.Client.Blog.Clients;

internal sealed class CommentsHttpClient : HttpClientBase, ICommentsClient
{
    private readonly HttpClientOptions options;

    public CommentsHttpClient(HttpClient httpClientFactory, IOptions<HttpClientOptions> options)
        : base(httpClientFactory)
    {
        this.options = options.Value;
    }

    public Task<ListAllResponse?> GetRootCommentsAsync(Guid postKey, int pageNumber, int pageSize)
    {
        return GetCommentsInternalAsync(postKey, null, pageNumber, pageSize);
    }

    public async Task<IReadOnlyList<CommentModel>?> GetChildCommentsAsync(Guid postKey, Guid parentKey)
    {
        var result = await GetCommentsInternalAsync(postKey, parentKey, null, null);
        return result?.Comments;
    }

    public async Task<CommentModel?> GetCommentAsync(Guid commentKey)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, options.Endpoints.Comments.BasePath);

        try
        {
            using (var response = await ClientFactory.SendAsync(request))
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

    public async Task<CreatedCommentModel?> CreateCommentAsync(Guid postKey, Guid? parentKey, string text)
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
            using (var response = await ClientFactory.SendAsync(request))
            {
                var message = response.EnsureSuccessStatusCode();

                using (var stream = await message.Content.ReadAsStreamAsync())
                {
                    var createdComment = await JsonSerializer.DeserializeAsync<CreatedCommentModel>(stream);

                    /*if (null != createdComment)
                    {
                        return new CreatedCommentModel(
                            createdComment.Key,
                            createdComment.PostKey,
                            createdComment.ParentKey,
                            createdComment.Text,
                            createdComment.Status,
                            createdComment.CreatedAt
                        );
                    }*/

                    return createdComment;
                }
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine(exception);
        }

        return null;
    }

    private async Task<ListAllResponse?> GetCommentsInternalAsync(Guid postKey, Guid? parentKey, int? pageNumber, int? pageSize)
    {
        var basePath = new Uri(options.Endpoints.Comments.BasePath, UriKind.Absolute);
        var relativeUri = new Uri(postKey.ToString("D"), UriKind.Relative);

        if (false == Uri.TryCreate(basePath, relativeUri, out var endpoint))
        {
            return null;
        }

        var queryString = new Dictionary<string, string?>();

        if (null != parentKey)
        {
            queryString.Add(nameof(parentKey), parentKey.Value.ToString("D"));
        }

        if (null != pageNumber)
        {
            queryString.Add(nameof(pageNumber), pageNumber.Value.ToString());
        }

        if (null != pageSize)
        {
            queryString.Add(nameof(pageSize), pageSize.Value.ToString());
        }
        
        var uri = QueryHelpers.AddQueryString(endpoint.AbsoluteUri, queryString);
        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        try
        {
            using (var response = await ClientFactory.SendAsync(request))
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
}