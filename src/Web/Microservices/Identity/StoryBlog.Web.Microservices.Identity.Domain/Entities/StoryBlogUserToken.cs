using Microsoft.AspNetCore.Identity;
using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public sealed class StoryBlogUserToken : IdentityUserToken<string>, IHasUserId<string>
{
}