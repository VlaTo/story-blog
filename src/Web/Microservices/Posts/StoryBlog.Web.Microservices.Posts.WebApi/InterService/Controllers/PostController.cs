using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPost;
using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Microservices.Posts.WebApi.InterService.Controllers;

/// <summary>
/// Work with posts internal controller.
/// </summary>
/// <param name="mediator"></param>
/// <param name="mapper"></param>
[ApiVersion("1.0-alpha")]
[Authorize]
[ApiController]
[Route("s2s/v{version:apiVersion}/[controller]")]
public sealed class PostController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// GET: /s2s/v/Post/
    /// Gets post model by <paramref name="key" /> for inter-service request.
    /// </summary>
    /// <param name="key">The post key.</param>
    /// <returns>The post as <see cref="PostModel" />.</returns>
    [ProducesResponseType(typeof(PostModel), StatusCodes.Status200OK)]
    [HttpGet("{key:guid:required}")]
    public async Task<IActionResult> GetPost([FromRoute] Guid key)
    {
        var query = new GetPostModelQuery(key, User);
        var result = await mediator.Send(query, HttpContext.RequestAborted);

        if (result.Succeeded)
        {
            var postModel = mapper.Map<PostModel>(result.Value!);
            return Ok(postModel);
        }

        return BadRequest(result.Error);
    }
}