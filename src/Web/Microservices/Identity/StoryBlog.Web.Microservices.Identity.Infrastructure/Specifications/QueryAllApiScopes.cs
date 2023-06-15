using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Specifications;

internal sealed class QueryAllApiScopes : SpecificationBase<Domain.Entities.ApiScope>
{
    public QueryAllApiScopes()
    {
        Includes.Add(x => x.UserClaims);
        Includes.Add(x => x.Properties);
        Options = SpecificationQueryOptions.NoTracking;
    }
}