using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoryBlog.Web.Microservices.Posts.Application.Handlers.DeletePost;
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
public sealed class PostController : Controller
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
    /// Gets existing post by slug or key(Guid).
    /// </summary>
    /// <param name="slugOrKey">The slug -or- key for post to get.</param>
    /// <returns></returns>
    [ProducesResponseType(typeof(PostModel), StatusCodes.Status200OK)]
    [HttpGet("{slugOrKey:required}", Name = RouteNames.GetPostRouteKey)]
    public async Task<IActionResult> GetPost([FromRoute] string slugOrKey)
    {
        var query = new GetPostQuery(slugOrKey, User);
        var getPostResult = await mediator.Send(query).ConfigureAwait(false);

        if (getPostResult.Succeeded)
        {
            var model = mapper.Map<PostModel>(getPostResult.Value);
            return Ok(model);
        }

        logger.LogDebug($"Post not found for slug: {slugOrKey}");

        return BadRequest();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="slugOrKey"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(PostModel), StatusCodes.Status200OK)]
    [HttpPut("{slugOrKey:required}")]
    public async Task<IActionResult> EditPost([FromRoute] string slugOrKey, [FromBody] EditPostRequest request)
    {
        var details = new EditPostDetails
        {
            Title = request.Title,
            Slug = request.Slug
        };

        var command = new EditPostCommand(slugOrKey, details, User);
        var editPostResult = await mediator.Send(command).ConfigureAwait(false);

        if (editPostResult.Succeeded)
        {
            var query = new GetPostQuery(slugOrKey, User);
            var queryPostResult = await mediator.Send(query).ConfigureAwait(false);

            if (queryPostResult.Succeeded)
            {
                var model = mapper.Map<PostModel>(queryPostResult.Value);
                return Ok(model);
            }
        }

        logger.LogDebug($"Post not found for key: {slugOrKey}");

        return BadRequest();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="slugOrKey"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(PostDeletedResponse), StatusCodes.Status200OK)]
    [HttpDelete("{slugOrKey:required}")]
    public async Task<IActionResult> DeletePost([FromRoute] string slugOrKey)
    {
        var command = new DeletePostCommand(slugOrKey, User);
        var result = await mediator.Send(command).ConfigureAwait(false);

        if (result.Succeeded)
        {
            if (result.IsOfT1)
            {
                // success
                var response = new PostDeletedResponse();
                return Ok(response);
            }

            if (result.IsOfT2)
            {
                // not found
                return NotFound();
            }
        }

        return StatusCode(StatusCodes.Status500InternalServerError, result.Error);
    }
}
