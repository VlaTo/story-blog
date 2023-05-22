namespace StoryBlog.Web.Microservices.Identity.Application.Validation;

public enum BearerTokenUsageType
{
    AuthorizationHeader = 0,
    PostBody = 1,
    QueryString = 2
}