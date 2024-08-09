using System.Collections.ObjectModel;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace StoryBlog.Web.Blazor.Markdown.Editor.Core;

public abstract class StyleBuilder
{
    public static StyleBuilder<TComponent> CreateFor<TComponent>() where TComponent : ComponentBase
    {
        return new StyleBuilder<TComponent>();
    }

    public abstract string Build(ComponentBase component);
}

public class StyleBuilder<TComponent> : StyleBuilder
    where TComponent : ComponentBase
{
    private readonly IList<CssDeclaration> styles;

    internal StyleBuilder()
    {
        styles = new List<CssDeclaration>();
    }

    public StyleBuilder<TComponent> DefineProperty(Action<ICssPropertyBuilder<TComponent>> configurator)
    {
        var builder = new InternalCssPropertyBuilder();

        configurator.Invoke(builder);
        
        styles.Add(builder.ToDeclaration());

        return this;
    }

    public override string Build(ComponentBase component)
    {
        return Build((TComponent)component);
    }

    private string Build(TComponent component)
    {
        const char whitespace = ' ';

        var style = new StringBuilder();

        foreach (var css in styles)
        {
            if (false == css.Condition.Invoke(component))
            {
                continue;
            }

            if (0 < style.Length)
            {
                style.Append(';').Append(whitespace);
            }

            style
                .Append(css.Property)
                .Append(':')
                .Append(whitespace);

            var value = css.Accessor.Invoke(component);

            style.Append(String.IsNullOrEmpty(value) ? "''" : value);

            for (var index = 0; index < css.Modifiers.Count; index++)
            {
                var modifier = css.Modifiers[index];

                if (false == modifier.Condition.Invoke(component))
                {
                    continue;
                }

                style.Append(whitespace).Append(modifier.Accessor.Invoke(component));
            }
        }

        return style.ToString();
    }

    #region CSS declaration

    private sealed record CssPropertyModifier(
        Predicate<TComponent> Condition,
        Func<TComponent, string> Accessor
    );

    private sealed record CssDeclaration(
        string Property,
        Predicate<TComponent> Condition,
        Func<TComponent, string?> Accessor,
        IReadOnlyList<CssPropertyModifier> Modifiers
    );

    #endregion

    #region CSS Property Builder

    private sealed class InternalCssPropertyBuilder : ICssPropertyBuilder<TComponent>
    {
        private string property;
        private Predicate<TComponent>? condition;
        private Func<TComponent, string?>? accessor;

        public CssDeclaration ToDeclaration()
        {
            return new CssDeclaration(
                property,
                condition ?? True,
                accessor!,
                new ReadOnlyCollection<CssPropertyModifier>([])
            );
        }

        public ICssPropertyBuilder<TComponent> Name(string value)
        {
            property = value;
            return this;
        }

        public ICssPropertyBuilder<TComponent> Condition(Predicate<TComponent> predicate)
        {
            condition = predicate;
            return this;
        }

        public ICssPropertyBuilder<TComponent> Value(Func<TComponent, string?> func)
        {
            accessor = func;
            return this;
        }

        private static bool True(TComponent _) => true;
    }

    #endregion
}