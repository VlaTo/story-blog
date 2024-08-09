using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace StoryBlog.Web.Blazor.Markdown.Editor.Core.Extensions;

public static class CssPropertyBuilderExtensions
{
    /*public static ICssPropertyBuilder<TComponent> ValueIfExists<TComponent>(this ICssPropertyBuilder<TComponent> builder, Expression<Func<TComponent, string?>> expression)
        where TComponent : ComponentBase
    {
        if (expression.Body is MemberExpression ma)
        {
            if (ma.NodeType == ExpressionType.MemberAccess)
            {
                var n = ma.Member.Name;
            }
        }

        return builder;
    }*/

    public static ICssPropertyBuilder<TComponent> Value<TComponent>(
        this ICssPropertyBuilder<TComponent> builder,
        string func)
        where TComponent : ComponentBase =>
        builder.Value(_ => func);

    public static ICssPropertyBuilder<TComponent> ValueIfExists<TComponent>(
        this ICssPropertyBuilder<TComponent> builder,
        Func<TComponent, string?> func)
        where TComponent : ComponentBase =>
        builder
            .Value(func)
            .Condition(
                x => false == String.IsNullOrEmpty(func.Invoke(x))
            );
}