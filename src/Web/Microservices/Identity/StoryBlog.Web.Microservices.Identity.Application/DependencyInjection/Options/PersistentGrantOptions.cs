namespace StoryBlog.Web.Microservices.Identity.Application.DependencyInjection.Options;

/// <summary>
/// Options for how persisted grants are persisted.
/// </summary>
public class PersistentGrantOptions
{
    /// <summary>
    /// Value protect the persisted grants "data" column.
    /// </summary>
    public bool DataProtectData
    {
        get;
        set;
    }

    public PersistentGrantOptions()
    {
        DataProtectData = true;
    }
}