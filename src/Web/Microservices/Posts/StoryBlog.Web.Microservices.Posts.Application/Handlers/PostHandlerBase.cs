using System.Security.Claims;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Application.Extensions;
using StoryBlog.Web.Common.Identity.Permission;
using StoryBlog.Web.Common.Result;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers;

public class PostHandlerBase : HandlerBase
{
    public ILogger Logger
    {
        get;
    }

    protected PostHandlerBase(ILogger logger)
    {
        Logger = logger;
    }
}