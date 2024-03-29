﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Microservices.Posts.Shared.Models;

[DataContract]
public sealed class EditPostRequest
{
    [Required]
    public string Title
    {
        get;
        set;
    }

    [Required]
    public string Slug
    {
        get;
        set;
    }
}