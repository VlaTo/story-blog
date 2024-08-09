namespace StoryBlog.Web.Blazor.Markdown.Editor.Core;

public interface ICssPropertyBuilder<out TComponent>
    where TComponent : class
{
    ICssPropertyBuilder<TComponent> Condition(Predicate<TComponent> predicate);

    ICssPropertyBuilder<TComponent> Name(string value);

    ICssPropertyBuilder<TComponent> Value(Func<TComponent, string?> func);
}