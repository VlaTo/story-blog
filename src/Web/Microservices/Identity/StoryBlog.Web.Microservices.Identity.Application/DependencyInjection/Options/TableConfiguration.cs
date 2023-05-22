﻿namespace StoryBlog.Web.Microservices.Identity.Application.DependencyInjection.Options;

/// <summary>
/// Class to control a table's name and schema.
/// </summary>
public class TableConfiguration
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string Name
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the schema.
    /// </summary>
    /// <value>
    /// The schema.
    /// </value>
    public string? Schema
    {
        get;
        set;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TableConfiguration"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    public TableConfiguration(string name)
        : this(name, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TableConfiguration"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="schema">The schema.</param>
    public TableConfiguration(string name, string? schema)
    {
        Name = name;
        Schema = schema;
    }
}