using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlimMessageBus;
using StoryBlog.Web.Common.Events;
using StoryBlog.Web.Microservices.Posts.Application.Services;

namespace StoryBlog.Web.Microservices.Posts.WebApi.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    /// <summary>
    /// Creates new post with parameters specified by <paramref name="request" />.
    /// </summary>
    /// <returns>
    /// 
    /// </returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost(nameof(SendPostCreatedEvent))]
    public async Task<IActionResult> SendPostCreatedEvent([FromServices] IMessageBus messageBus)
    {
        var createdEvent = new NewPostCreatedEvent(Guid.NewGuid(), DateTimeOffset.Now, "Test");

        await messageBus.Publish(createdEvent, cancellationToken: HttpContext.RequestAborted);

        return Ok();
    }

    /// <summary>
    /// Creates new post with parameters specified by <paramref name="request" />.
    /// </summary>
    /// <returns>
    /// 
    /// </returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost(nameof(QueueNewTask))]
    public async Task<IActionResult> QueueNewTask(
        [FromForm] double seconds,
        [FromServices] IBlogPostProcessingManager workManager,
        [FromServices] ILogger<TestController> logger)
    {
        //var backgroundTask = await workManager.AddPostTaskAsync(seconds, cancellationToken: HttpContext.RequestAborted);

        //logger.LogDebug($"New background task with ID: {backgroundTask.Id:D} queued");

        return Ok();
    }
}
