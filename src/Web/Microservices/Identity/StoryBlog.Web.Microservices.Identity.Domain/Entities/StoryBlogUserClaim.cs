using Microsoft.AspNetCore.Identity;
using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public class StoryBlogUserClaim : IdentityUserClaim<string>, IEntity, IHasId<int>, IHasUserId<string>
{
}