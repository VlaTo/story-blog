using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;
using System.Text;

namespace StoryBlog.Web.Blazor.Markdown.Editor.Core;

public abstract class ClassBuilder
{
    public static ClassBuilder<TComponent> CreateFor<TComponent>(string? classNamePrefix = null, string? componentPrefix = null) where TComponent : ComponentBase
    {
        return new ClassBuilder<TComponent>(classNamePrefix, componentPrefix);
    }

    public abstract string Build(ComponentBase component, string? extras = "", bool addClassNamePrefix = true);
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="TComponent"></typeparam>
public class ClassBuilder<TComponent> : ClassBuilder where TComponent : ComponentBase
{
    private const char ClassNameSeparator = ' ';
    private const string DashSeparator = "-";

    private readonly string? classNamePrefix;
    private readonly StringBuilder builder;
    private readonly IList<ClassDefinition> classDefinitions;

    public ClassBuilder(string? classNamePrefix = null, string? componentPrefix = null)
    {
        this.classNamePrefix = classNamePrefix;

        builder = new StringBuilder();
        classDefinitions = new List<ClassDefinition>();

        if (false == String.IsNullOrWhiteSpace(componentPrefix))
        {
            if (false == String.IsNullOrWhiteSpace(this.classNamePrefix))
            {
                this.classNamePrefix += (DashSeparator + componentPrefix);
            }
            else
            {
                throw new ArgumentException("", nameof(classNamePrefix));
            }
        }
    }

    public override string Build(ComponentBase component, string? extras = "", bool addClassNamePrefix = true)
    {
        return Build((TComponent)component, extras, addClassNamePrefix);
    }

    public string Build(TComponent component, string? extras = "", bool addClassNamePrefix = true)
    {
        if (0 == classDefinitions.Count)
        {
            return String.Empty;
        }

        builder.Clear();

        if (addClassNamePrefix && false == String.IsNullOrEmpty(classNamePrefix))
        {
            builder.Append(classNamePrefix);
        }

        foreach (var definition in classDefinitions)
        {
            if (false == definition.Condition.Invoke(component))
            {
                continue;
            }

            if (0 < builder.Length)
            {
                builder.Append(ClassNameSeparator);
            }

            var hasPrefix = false == String.IsNullOrWhiteSpace(definition.Prefix);

            if (hasPrefix)
            {
                builder.Append(definition.Prefix);
            }

            var count = 0;

            foreach (var modifier in definition.Modifiers)
            {
                if (false == modifier.Condition.Invoke(component))
                {
                    continue;
                }

                if (0 < count || hasPrefix)
                {
                    builder.Append(DashSeparator);
                }

                var value = modifier.Accessor.Invoke(component);

                builder.Append(value);
                count++;
            }

            if (0 < definition.Modifiers.Count || hasPrefix)
            {
                builder.Append(definition.PrefixSeparator);
            }

            if (null != definition.Accessor)
            {
                builder.Append(definition.Accessor.Invoke(component));
            }
        }

        if (false == String.IsNullOrWhiteSpace(extras))
        {
            builder.Append(ClassNameSeparator).Append(extras);
        }

        return builder.ToString();
    }

    public ClassBuilder<TComponent> DefineClass(Action<IClassBuilder<TComponent>> configurator)
    {
        if (null == configurator)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        var classBuilder = new InternalClassBuilder();

        configurator.Invoke(classBuilder);

        classDefinitions.Add(classBuilder.Build(classNamePrefix));

        return this;
    }

    #region ClassDefinition

    /// <summary>
    /// 
    /// </summary>
    private sealed record ClassDefinition(
        string? Prefix,
        Func<TComponent, string>? Accessor,
        IReadOnlyList<ClassModifier> Modifiers,
        Predicate<TComponent> Condition,
        string PrefixSeparator
    );

    #endregion

    #region ClassModifier

    /// <summary>
    /// 
    /// </summary>
    private sealed record ClassModifier(
        Func<TComponent, string> Accessor,
        Predicate<TComponent> Condition
    );

    #endregion

    #region InternalClassBuilder

    /// <summary>
    /// 
    /// </summary>
    private class InternalClassBuilder : IClassBuilder<TComponent>
    {
        private bool noPrefix;
        private Predicate<TComponent>? condition;
        private Func<TComponent, string>? accessor;
        private readonly IList<ClassModifier> modifiers = new List<ClassModifier>();

        public IClassBuilder<TComponent> NoPrefix()
        {
            noPrefix = true;
            return this;
        }

        public IClassBuilder<TComponent> Modifier(Func<TComponent, string> func, Predicate<TComponent> predicate)
        {
            modifiers.Add(new ClassModifier(func, predicate));
            return this;
        }

        public IClassBuilder<TComponent> Name(Func<TComponent, string> valueAccessor)
        {
            accessor = valueAccessor;
            return this;
        }

        public IClassBuilder<TComponent> Condition(Predicate<TComponent> predicate)
        {
            if (null != condition)
            {
                throw new ArgumentException("", nameof(predicate));
            }

            condition = predicate;

            return this;
        }

        internal ClassDefinition Build(string? prefix)
        {
            return new ClassDefinition(
                false == noPrefix ? prefix : null,
                accessor,
                new ReadOnlyCollection<ClassModifier>(modifiers),
                condition ?? True,
                DashSeparator
            );
        }

        private static bool True(TComponent _) => true;
    }

    #endregion
}