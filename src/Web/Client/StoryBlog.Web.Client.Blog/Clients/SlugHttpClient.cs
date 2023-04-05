using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Client.Blog.Clients.Interfaces;
using StoryBlog.Web.Client.Blog.Configuration;
using StoryBlog.Web.Microservices.Posts.Shared.Models;
using System.Diagnostics;
using System.Text.Json;

namespace StoryBlog.Web.Client.Blog.Clients;

internal sealed class SlugHttpClient : HttpClientBase, ISlugClient
{
    private readonly HttpClientOptions options;

    public SlugHttpClient(HttpClient httpClient, IOptions<HttpClientOptions> options)
        : base(httpClient)
    {
        this.options = options.Value;
    }

    public async Task<GeneratedSlugModel?> GenerateSlugAsync(string title)
    {
        var uri = QueryHelpers.AddQueryString(
            options.Endpoints.Slugs.BasePath,
            new Dictionary<string, string>
            {
                { "title", title }
            }
        );

        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        try
        {
            using (var response = await Client.SendAsync(request))
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
        var basePath = new Uri(options.Endpoints.Slug.BasePath);
        var relativeUri = new Uri(slug,UriKind.Relative);

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