using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoryBlog.Web.Microservices.Comments.Shared.Models;

namespace StoryBlog.Web.Microservices.Comments.WebApi.Controllers;

/// <summary>
/// 
/// </summary>
[ApiVersion("1.0-alpha")]
[AllowAnonymous]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public sealed class CommentsController : Controller
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly ILogger<CommentsController> logger;

    public CommentsController(
        IMediator mediator,
        IMapper mapper,
        ILogger<CommentsController> logger)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.logger = logger;
    }

    /// <summary>
    /// Lists available posts to user.
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(typeof(CommentsListResponse), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(void))]
    [HttpGet("{key:guid:required}")]
    public Task<IActionResult> ListAll([FromRoute] Guid key, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 30)
    {
        /*var query = new GetPostsQuery(pageNumber, pageSize, includeAll: true);
        var result = await mediator.Send(query);

        if (result.IsSuccess())
        {
            var models = mapper.Map<IReadOnlyCollection<PostModel>>(result.Posts);
            return Ok(new ListAllResponse
            {
                Posts = models,
                PageNumber = pageNumber,
                PageSize = pageSize
            });
        }

        return BadRequest();*/

        var result = Ok(new CommentsListResponse
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        });

        return Task.FromResult<IActionResult>(result);
    }
}