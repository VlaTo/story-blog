using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoryBlog.Web.Microservices.Posts.Application.Handlers.EditPost;
using StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPost;
using StoryBlog.Web.Microservices.Posts.Application.Models;
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
public class PostController : Controller
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly ILogger<PostController> logger;

    public PostController(
        IMediator mediator,
        IMapper mapper,
        ILogger<PostController> logger)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(PostModel), StatusCodes.Status200OK)]
    [HttpGet("{key:guid:required}", Name = RouteNames.GetPostRouteKey)]
    public async Task<IActionResult> GetPost([FromRoute] Guid key)
    {
        var query = new GetPostQuery(key, User);
        var postResult = await mediator.Send(query).ConfigureAwait(false);

        if (postResult.IsSuccess())
        {
            var model = mapper.Map<PostModel>(postResult.Post);
            return Ok(model);
        }

        logger.LogDebug($"Post not found for slug: {key}");

        return BadRequest();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(PostModel), StatusCodes.Status200OK)]
    [HttpPut("{key:guid:required}")]
    public async Task<IActionResult> EditPost([FromRoute] Guid key, [FromBody] EditPostRequest request)
    {
        var details = new EditPostDetails
        {
            Title = request.Title,
            Slug = request.Slug
        };

        var command = new EditPostCommand(key, details, User);
        var success = await mediator.Send(command).ConfigureAwait(false);

        if (success)
        {
            var query = new GetPostQuery(key, User);
            var postResult = await mediator.Send(query).ConfigureAwait(false);

            if (postResult.IsSuccess())
            {
                var model = mapper.Map<PostModel>(postResult.Post);
                return Ok(model);
            }
        }

        logger.LogDebug($"Post not found for key: {key}");

        return BadRequest();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(PostModel), StatusCodes.Status200OK)]
    [HttpDelete("{key:guid:required}")]
    public async Task<IActionResult> DeletePost([FromRoute] Guid key)
    {
        /*var command = new EditPostCommand(request.Title, request.Key, User);
        var success = await mediator.Send(command).ConfigureAwait(false);

        if (success)
        {
            var query = new GetPostQuery(key, User);
            var postResult = await mediator.Send(query).ConfigureAwait(false);

            if (postResult.IsSuccess())
            {
                var model = mapper.Map<PostModel>(postResult.Post);
                return Ok(model);
            }
        }

        logger.LogDebug($"Post not found for key: {key}");

        return BadRequest();*/

        return StatusCode(StatusCodes.Status501NotImplemented);
    }
}
