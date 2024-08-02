namespace StoryBlog.Web.Identity.Client;

public sealed class AuthorityValidationResult
{
    public static readonly AuthorityValidationResult SuccessResult = new(true, null);

    public string ErrorMessage { get; }

    public bool Success { get; }

    private AuthorityValidationResult(bool success, string? message)
    {
        if (!success && String.IsNullOrEmpty(message))
        {
            throw new ArgumentException("A message must be provided if success=false.", nameof(message));
        }

        ErrorMessage = message!;
        Success = success;
    }

    public static AuthorityValidationResult CreateError(string message) => new(false, message);

    public override string ToString() => Success ? "success" : ErrorMessage;
}