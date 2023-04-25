﻿using StoryBlog.Web.Common.Domain.Specifications;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Domain.Specifications;

public sealed class FindPostByKeySpecification : SpecificationBase<Post>
{
    public FindPostByKeySpecification(Guid key)
    {
        Criteria = entity => entity.Key == key;
        Includes.Add(entity => entity.Slug);
        Includes.Add(entity => entity.CommentsCounter);
    }
}