using StoryBlog.Web.Microservices.Identity.Application.Models.Responses;
using StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Models;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Requests;

namespace StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Generators;

/// <summary>
/// Interface for determining if user must login or consent when making requests to the authorization endpoint.
/// </summary>
public interface IAuthorizeInteractionResponseGenerator
{
    /// <summary>
    /// Processes the interaction logic.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="consent">The consent.</param>
    /// <returns></returns>
    Task<InteractionResponse> ProcessInteractionAsync(ValidatedAuthorizeRequest request, ConsentResponse? consent = null);
}