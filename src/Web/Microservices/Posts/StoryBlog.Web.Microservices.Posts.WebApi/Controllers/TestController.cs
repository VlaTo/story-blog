using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlimMessageBus;
using StoryBlog.Web.Microservices.Posts.Application.Services;
using StoryBlog.Web.Microservices.Posts.Events;

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
        var createdEvent = new NewPostCreatedEvent
        {
            PostKey = Guid.NewGuid(),
            Slug = "test-post-slug",
            AuthorId = "Test",
            Created = DateTimeOffset.Now
        };

        await messageBus.Publish(createdEvent, cancellationToken: HttpContext.RequestAborted);

        return Ok();
    }

    /// <summary>
    /// Creates new post with parameters specified by <paramref name="postKey" />.
    /// </summary>
    /// <returns>
    /// OK result
    /// </returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost(nameof(QueueNewTask))]
    public async Task<IActionResult> QueueNewTask(
        [FromForm] Guid postKey,
        [FromServices] IBlogPostProcessingManager workManager,
        [FromServices] ILogger<TestController> logger)
    {
        var backgroundTask = await workManager.QueuePostProcessingTaskAsync(postKey, cancellationToken: HttpContext.RequestAborted);
        
        logger.LogDebug($"New background task with Key: {backgroundTask.TaskKey:D} queued");

        return Ok();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet(nameof(ObserveRoutes))]
    public Task<IActionResult> ObserveRoutes(
        [FromServices] IEnumerable<EndpointDataSource> endpointSources,
        [FromServices] ILogger<TestController> logger)
    {
        var paths = new List<KeyValuePair<string, string[]>>();

        /*var context = new VirtualPathContext(HttpContext, RouteData.DataTokens, RouteData.Values);

        foreach (var router in RouteData.Routers)
        {
            var pathData = router.GetVirtualPath(context);
            paths.Add(pathData?.VirtualPath ?? "(none)");
        }*/

        foreach (var endpointSource in endpointSources)
        {
            var temp = new string[endpointSource.Endpoints.Count];

            for (var index = 0; index < endpointSource.Endpoints.Count; index++)
            {
                if (endpointSource.Endpoints[index] is RouteEndpoint re)
                {
                    temp[index] = re.RoutePattern.RawText!;
                }
                else
                {
                    temp[index] = endpointSource.Endpoints[index].DisplayName;
                }
            }

            paths.Add(new KeyValuePair<string, string[]>(endpointSource.GetHashCode().ToString(), temp));
        }

        return Task.FromResult<IActionResult>(Ok(new
        {
            Paths = paths.ToArray()
        }));
    }
}
