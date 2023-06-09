﻿namespace StoryBlog.Web.Microservices.Posts.WebApi.Configuration;

public sealed class PostLocationProviderOptions
{
    internal const string SectionName = "LocationProvider";

    public bool UseUrlHelper { get; set; }

    public string? ExternalUrlTemplate { get; set; }
}