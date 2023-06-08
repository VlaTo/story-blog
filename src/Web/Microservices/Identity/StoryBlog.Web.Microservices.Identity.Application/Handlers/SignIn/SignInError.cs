namespace StoryBlog.Web.Microservices.Identity.Application.Handlers.SigningIn;

public enum SignInError
{
    Unknown = -4,
    NotFound,
    NotAllowed,
    LockedOut,
    Unauthorized
}