using StoryBlog.Web.Microservices.Identity.Application.Validation.Contexts;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation;

// todo: ciba perhaps make a default IBackchannelAuthenticationUserValidator based on the idtokenhint claims?
// and maybe it calls into the profile service?

/// <summary>
/// Nop implementation of IBackchannelAuthenticationUserValidator.
/// </summary>
public sealed class NopBackchannelAuthenticationUserValidator : IBackchannelAuthenticationUserValidator
{
    public Task<BackchannelAuthenticationUserValidationResult> ValidateRequestAsync(BackchannelAuthenticationUserValidatorContext userValidatorContext) =>
        Task.FromResult(new BackchannelAuthenticationUserValidationResult
        {
            Error = "not implemented"
        });
}