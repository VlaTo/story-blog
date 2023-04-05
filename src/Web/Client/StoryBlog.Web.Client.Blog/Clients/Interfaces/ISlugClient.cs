using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Client.Blog.Clients.Interfaces;

public interface ISlugClient
{
    Task<GeneratedSlugModel?> GenerateSlugAsync(string title);

    Task<PostReferenceModel?> FetchPostReferenceAsync(string slug);
}