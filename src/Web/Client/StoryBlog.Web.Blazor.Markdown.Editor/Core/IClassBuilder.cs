﻿using Microsoft.AspNetCore.Components;

namespace StoryBlog.Web.Blazor.Markdown.Editor.Core;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TComponent"></typeparam>
public interface IClassBuilder<out TComponent> where TComponent : ComponentBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IClassBuilder<TComponent> NoPrefix();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="func"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    IClassBuilder<TComponent> Modifier(Func<TComponent, string> func, Predicate<TComponent> condition);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    IClassBuilder<TComponent> Name(Func<TComponent, string> func);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="condition"></param>
    /// <returns></returns>
    IClassBuilder<TComponent> Condition(Predicate<TComponent> condition);
}