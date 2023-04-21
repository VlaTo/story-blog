using Microsoft.AspNetCore.Components;
using MudBlazor.Utilities;

namespace StoryBlog.Web.Client.Blog.Components;

public enum DateTimeMode
{
    DateOnly,
    TimeOnly,
    RelativeFromCurrent
}

public partial class DateTimeView
{
    protected string Classname => new CssBuilder("storyblog-datetime")
        .AddClass(Class)
        .Build();

    [Parameter]
    public string? Class
    {
        get; 
        set;
    }

    [Parameter]
    public DateTime? DateTime
    {
        get;
        set;
    }

    [Parameter]
    public DateTimeMode Mode
    {
        get;
        set;
    }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object?> UserAttributes
    {
        get; 
        set;
    }

    public DateTimeView()
    {
        Mode = DateTimeMode.DateOnly;
    }

    private string FormatValue()
    {
        if (null != DateTime)
        {
            switch (Mode)
            {
                case DateTimeMode.DateOnly:
                {
                    return DateTime.Value.ToShortDateString();
                }

                case DateTimeMode.TimeOnly:
                {
                    return DateTime.Value.ToShortTimeString();
                }

                case DateTimeMode.RelativeFromCurrent:
                {
                    var now = System.DateTime.UtcNow;

                    if (now.Date == DateTime.Value.Date)
                    {
                        return DateTime.Value.ToShortTimeString();
                    }

                    return DateTime.Value.ToString("g");
                }
            }
        }

        return String.Empty;
    }
}