using Microsoft.AspNetCore.Http;
using StoryBlog.Web.Microservices.Identity.Application.Hosting;
using System.Net;
using System.Text.Json.Serialization;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Models;

namespace StoryBlog.Web.Microservices.Identity.Application.Endpoints.Results;

public sealed class TokenErrorResult : IEndpointResult
{
    public TokenErrorResponse Response
    {
        get;
        init;
    }

    public TokenErrorResult(TokenErrorResponse error)
    {
        Response = error;
    }

    public async Task ExecuteAsync(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.SetNoCache();

        var dto = new Result
        {
            error = Response.Error,
            error_description = Response.ErrorDescription,

            custom = Response.Custom
        };

        await context.Response.WriteJsonAsync(dto);
    }

    /// <summary>
    /// 
    /// </summary>
    internal class Result
    {
        public string error
        {
            get;
            set;
        }

        public string error_description
        {
            get;
            set;
        }

        [JsonExtensionData]
        public Dictionary<string, object> custom
        {
            get;
            set;
        }
    }
}