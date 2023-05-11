using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using MudBlazor.Utilities;
using StoryBlog.Web.Client.Blog.Core;

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
        .AddClass("mud-typography-" + Typo.ToDescriptionString())
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
    [Category("Appearance")]
    public Typo Typo
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
        Typo = Typo.body1;
        Mode = DateTimeMode.DateOnly;
    }

    private string FormatValue()
    {
        if (null != DateTime)
        {
            var dateTime = DateTime.Value;

            switch (Mode)
            {
                case DateTimeMode.DateOnly:
                {
                    return dateTime.ToShortDateString(); // d
                }

                case DateTimeMode.TimeOnly:
                {
                    return dateTime.ToShortTimeString(); // t
                }

                case DateTimeMode.RelativeFromCurrent:
                {
                    var now = System.DateTime.UtcNow;
                    var timeSpanQuantity = new RussianTimeSpanQuantity(Localizer, "CommentAge");

                    return timeSpanQuantity.GetQuantityString(dateTime, now.Date - dateTime.Date);
                }
            }
        }

        return String.Empty;
    }
}