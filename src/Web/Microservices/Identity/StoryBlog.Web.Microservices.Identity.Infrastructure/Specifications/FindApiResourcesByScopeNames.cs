using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Specifications;

internal sealed class FindApiResourcesByScopeNames : SpecificationBase<Domain.Entities.ApiResource>
{
    public FindApiResourcesByScopeNames(string[] names)
    {
        Criteria = resource => resource.Scopes.Any(scope => names.Contains(scope.Scope));
        Includes.Add(resource => resource.Secrets);
        Includes.Add(resource => resource.Scopes);
        Includes.Add(resource => resource.UserClaims);
        Includes.Add(resource => resource.Properties);
        Options = SpecificationQueryOptions.NoTracking | SpecificationQueryOptions.SplitQuery;
    }
}