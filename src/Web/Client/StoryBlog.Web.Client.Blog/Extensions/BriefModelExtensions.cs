﻿using StoryBlog.Web.Client.Blog.Models;
using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Client.Blog.Extensions;

internal static class BriefModelExtensions
{
    public static BriefPostModel ToModel(this BriefModel brief, PostState state) =>
        new(
            brief.Key,
            brief.Slug,
            brief.Title,
            brief.Author,
            brief.Status,
            brief.Text,
            brief.AllowedActions,
            state,
            brief.CommentsCount,
            brief.CreatedAt
        );
}