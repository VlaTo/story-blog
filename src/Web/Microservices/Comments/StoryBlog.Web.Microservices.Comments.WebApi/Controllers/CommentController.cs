using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoryBlog.Web.Microservices.Comments.Shared.Models;
using StoryBlog.Web.Microservices.Comments.WebApi.Core;

namespace StoryBlog.Web.Microservices.Comments.WebApi.Controllers;

/// <summary>
/// 
/// </summary>
[ApiVersion("1.0-alpha")]
[AllowAnonymous]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class CommentController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly ILogger<CommentsController> logger;

    public CommentController(
        IMediator mediator,
        IMapper mapper,
        ILogger<CommentsController> logger)
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
    [ProducesResponseType(typeof(CommentModel), StatusCodes.Status200OK)]
    [HttpGet("{key:guid:required}", Name = RouteNames.GetCommentRouteKey)]
    public async Task<IActionResult> GetComment([FromRoute] Guid key)
    {
        /*var query = new GetPostQuery(key, User);
        var postResult = await mediator.Send(query).ConfigureAwait(false);

        if (postResult.IsSuccess())
        {
            var model = mapper.Map<PostModel>(postResult.Post);
            return Ok(model);
        }*/

        logger.LogDebug($"Post not found for slug: {key}");

        return BadRequest();
    }
}
