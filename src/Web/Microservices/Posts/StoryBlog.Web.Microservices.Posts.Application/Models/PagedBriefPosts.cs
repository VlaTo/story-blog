namespace StoryBlog.Web.Microservices.Posts.Application.Models;

public sealed record PagedBriefPosts(
    IReadOnlyList<Brief> Posts,
    string? PreviousPageToken,
    string? NextPageToken
);