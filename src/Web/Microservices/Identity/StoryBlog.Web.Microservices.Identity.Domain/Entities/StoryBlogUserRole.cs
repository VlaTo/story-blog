﻿using Microsoft.AspNetCore.Identity;
using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public sealed class StoryBlogUserRole : IdentityUserRole<string>, IEntity, IHasUserId<string>
{
}