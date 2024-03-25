using StoryBlog.Web.Common.Domain.Specifications;
using StoryBlog.Web.Microservices.Comments.Domain.Entities;

namespace TestProject1.Controllers.Comments;

internal sealed class EmptySpecification : SpecificationBase<Comment>
{
    public EmptySpecification()
    {
    }
}