using System.Diagnostics;
using System.Net.Mime;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoryBlog.Web.Microservices.Posts.Application.Handlers.CreatePost;
using StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPost;
using StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPosts;
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
public class PostsController : Controller
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly ILocationProvider locationProvider;
    private readonly ILogger<PostsController> logger;

    public PostsController(
        IMediator mediator,
        IMapper mapper,
        ILocationProvider locationProvider,
        ILogger<PostsController> logger)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.locationProvider = locationProvider;
        this.logger = logger;
    }

    /// <summary>
    /// Lists available posts to user.
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(typeof(ListAllResponse), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(void))]
    [HttpGet]
    public async Task<IActionResult> ListAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 30)
    {
        var query = new GetPostsQuery(User, pageNumber, pageSize, includeAll: true);
        var result = await mediator.Send(query);

        if (result.IsSuccess)
        {
            var models = mapper.Map<IReadOnlyCollection<BriefModel>>(result.Value);
            return Ok(new ListAllResponse
            {
                Posts = models,
                PageNumber = pageNumber,
                PageSize = pageSize
            });
        }

        return BadRequest();
    }

    /// <summary>
    /// Creates new post with parameters specified by <paramref name="request" />.
    /// </summary>
    /// <param name="request">Parameters to create.</param>
    /// <returns>
    /// 
    /// </returns>
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(CreatedPostModel), StatusCodes.Status201Created)]
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
    {
        if (false == ModelState.IsValid)
        {
            return BadRequest();
        }

        var createPostDetails = new Application.Models.CreatePostDetails
        {
            Title = request.Title,
            Slug = request.Slug,
            Text = "",
            Brief = ""
        };

        var command = new CreatePostCommand(createPostDetails, User);
        var createdPostResult = await mediator.Send(command).ConfigureAwait(false);

        if (createdPostResult.IsSuccess)
        {
            var query = new GetPostQuery(createdPostResult.Value, User);
            var queryPostResult = await mediator.Send(query).ConfigureAwait(false);

            if (queryPostResult.IsSuccess)
            {
                var location = locationProvider.GetPostUri(ControllerContext, RouteNames.GetPostRouteKey, createdPostResult.Value);

                if (null != location)
                {
                    var model = mapper.Map<CreatedPostModel>(queryPostResult.Value);
                    return Created(location, model);
                }

                logger.LogWarning($"Failed to build location for new post with key: {createdPostResult:D}");
            }
            else
            {
                logger.LogWarning($"Failed to retrieve post with key: {createdPostResult:D}");
            }
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}
