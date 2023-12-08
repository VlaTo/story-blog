using StoryBlog.Web.Common.Domain.Specifications;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Domain.Specifications;

public sealed class TailPostsSpecification : SpecificationBase<Post>
{
    public TailPostsSpecification(DateTimeOffset dateTime, int postsCount, bool authenticated)
    {
        Criteria = entity => entity.CreateAt > dateTime && null == entity.DeletedAt && (entity.IsPublic || authenticated);
        Includes.Add(x => x.Slug);
        Includes.Add(x => x.Content);
        OrderBy.Add(entity => entity.CreateAt);
        OrderBy.Add(entity => entity.Title);
        Take = postsCount;
    }
}