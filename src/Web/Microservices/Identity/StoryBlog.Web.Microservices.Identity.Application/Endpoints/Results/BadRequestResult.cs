using Microsoft.AspNetCore.Http;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Hosting;

namespace StoryBlog.Web.Microservices.Identity.Application.Endpoints.Results;

internal sealed class BadRequestResult : IEndpointResult
{
    public string? Error
    {
        get; 
        set;
    }

    public string? ErrorDescription
    {
        get; 
        set;
    }

    public BadRequestResult(
        string? error = null,
        string? errorDescription = null)
    {
        Error = error;
        ErrorDescription = errorDescription;
    }

    public async Task ExecuteAsync(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.SetNoCache();

        if (Error.IsPresent())
        {
            var dto = new ResultDto
            {
                error = Error,
                error_description = ErrorDescription
            };

            await context.Response.WriteJsonAsync(dto);
        }
    }

    #region Result

    internal class ResultDto
    {
        public string? error
        {
            get; 
            set;
        }

        public string? error_description
        {
            get; 
            set;
        }
    }

    #endregion
}