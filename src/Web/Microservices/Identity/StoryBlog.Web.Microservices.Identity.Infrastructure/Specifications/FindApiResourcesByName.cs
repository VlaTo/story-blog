using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Specifications;

internal sealed class FindApiResourcesByName : SpecificationBase<Domain.Entities.ApiResource>
{
    public FindApiResourcesByName(string[] names)
    {
        Criteria = resource => names.Contains(resource.Name);
        Includes.Add(resource => resource.Secrets);
        Includes.Add(resource => resource.Scopes);
        Includes.Add(resource => resource.UserClaims);
        Includes.Add(resource => resource.Properties);
        Options = SpecificationQueryOptions.NoTracking;
    }
}