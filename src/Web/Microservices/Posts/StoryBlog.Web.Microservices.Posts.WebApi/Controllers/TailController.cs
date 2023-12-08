using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoryBlog.Web.Microservices.Posts.Application.Handlers.GetTailPosts;
using StoryBlog.Web.Microservices.Posts.Shared.Models;
using StoryBlog.Web.Microservices.Posts.WebApi.Core;

namespace StoryBlog.Web.Microservices.Posts.WebApi.Controllers;

/// <summary>
/// 
/// </summary>
[ApiVersion("1.0-alpha")]
[AllowAnonymous]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public sealed class TailController : Controller
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly ILogger<TailController> logger;

    public TailController(
        IMediator mediator,
        IMapper mapper,
        ILogger<TailController> logger)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.logger = logger;
    }

    /// <summary>
    /// Lists available posts to user.
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(typeof(IReadOnlyCollection<BriefModel>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(void))]
    [HttpGet("{postKey:guid}/{postsCount:int}")]
    public async Task<IActionResult> ListAll([FromRoute] Guid postKey, [FromRoute] int postsCount = 1)
    {
        var query = new GetTailPostsQuery(User, postKey, postsCount, includeAll: true);
        var result = await mediator.Send(query);

        if (result.Succeeded)
        {
            var posts = mapper.Map<IReadOnlyCollection<BriefModel>>(result.Value);
            return Ok(posts);
        }

        return BadRequest();
    }
}