﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Client.Blog.Models;

[DataContract]
public sealed class EditPostModel : PostDetailsModel
{
    [Required]
    public string Content
    {
        get;
        set;
    }
}