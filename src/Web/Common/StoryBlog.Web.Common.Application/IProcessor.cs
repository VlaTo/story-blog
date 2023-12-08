namespace StoryBlog.Web.Common.Application;

public interface IProcessor<in TModel>
{
    Task ProcessAsync(TModel model, CancellationToken cancellationToken);
}