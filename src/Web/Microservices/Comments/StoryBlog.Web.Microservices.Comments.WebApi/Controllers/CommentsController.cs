using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoryBlog.Web.Microservices.Comments.Application.Handlers.CreateComment;
using StoryBlog.Web.Microservices.Comments.Application.Handlers.GetComment;
using StoryBlog.Web.Microservices.Comments.Application.Handlers.GetComments;
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
public sealed class CommentsController : Controller
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;
    private readonly ILocationProvider locationProvider;
    private readonly ILogger<CommentsController> logger;

    public CommentsController(
        IMediator mediator,
        IMapper mapper,
        ILocationProvider locationProvider,
        ILogger<CommentsController> logger)
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
    [HttpGet("{postKey:guid:required}")]
    public async Task<IActionResult> ListAll([FromRoute] Guid postKey, [FromQuery] Guid? parentKey = null, [FromQuery] int pageNumber = -1, [FromQuery] int pageSize = 0)
    {
        var query = new GetCommentsQuery(postKey, parentKey, pageNumber, pageSize, includeAll: true);
        var result = await mediator.Send(query);

        if (result.Succeeded)
        {
            var comments = mapper.Map<IReadOnlyList<CommentModel>>(result.Value);
            return Ok(new ListAllResponse
            {
                Comments = comments,
                PageNumber = pageNumber,
                PageSize = pageSize
            });
        }

        return BadRequest();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(CreatedCommentModel), StatusCodes.Status201Created)]
    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest request)
    {
        if (false == ModelState.IsValid)
        {
            return BadRequest();
        }

        var createCommentDetails = new Application.Models.CreateCommentDetails
        {
            PostKey = request.PostKey,
            ParentKey = request.ParentKey,
            Text = request.Text
        };

        var command = new CreateCommentCommand(createCommentDetails, User);
        var createCommentResult = await mediator.Send(command).ConfigureAwait(false);

        if (createCommentResult.Succeeded)
        {
            var query = new GetCommentQuery(createCommentResult.Value, User);
            var queryCommentResult = await mediator.Send(query).ConfigureAwait(false);

            if (queryCommentResult.Succeeded)
            {
                var location = locationProvider.GetCommentUri(ControllerContext, RouteNames.GetCommentRouteKey, createCommentResult.Value);

                if (null != location)
                {
                    var model = mapper.Map<CreatedCommentModel>(queryCommentResult.Value);
                    return Created(location, model);
                }

                logger.LogWarning($"Failed to build location for new comment with key: {createCommentResult.Value:D}");
            }
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}