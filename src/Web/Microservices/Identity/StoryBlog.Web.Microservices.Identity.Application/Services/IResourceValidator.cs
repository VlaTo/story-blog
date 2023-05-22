using StoryBlog.Web.Microservices.Identity.Application.Models;
using StoryBlog.Web.Microservices.Identity.Application.Models.Requests;

namespace StoryBlog.Web.Microservices.Identity.Application.Services;

/// <summary>
/// Validates requested resources (scopes and resource indicators)
/// </summary>
public interface IResourceValidator
{
    /// <summary>
    /// Validates the requested resources for the client.
    /// </summary>
    Task<ResourceValidationResult> ValidateRequestedResourcesAsync(ResourceValidationRequest request);
}