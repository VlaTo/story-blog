using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
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

    [Inject]
    internal IStringLocalizer<DateTimeView> Localizer
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
                    return DateTime.Value.ToShortDateString(); // d
                }

                case DateTimeMode.TimeOnly:
                {
                    return DateTime.Value.ToShortTimeString(); // t
                }

                case DateTimeMode.RelativeFromCurrent:
                {
                    var now = System.DateTime.UtcNow;
                    return FormatDateTime(
                        now.Date == DateTime.Value.Date ? "TodayDateTimeFormat" : "GeneralDateTimeFormat"
                    );
                }
            }
        }

        return String.Empty;
    }

    private string FormatDateTime(string formatKey)
    {
        var format = Localizer[formatKey];
        return String.Format(format, DateTime!.Value);
    }
}