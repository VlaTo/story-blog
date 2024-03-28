using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using MudBlazor.Utilities;
using StoryBlog.Web.Client.Core.Localization;

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
    public DateTimeOffset? DateTime
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
    } = Typo.body1;

    [Parameter]
    public DateTimeMode Mode
    {
        get;
        set;
    } = DateTimeMode.DateOnly;

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

    private string FormatValue()
    {
        if (null != DateTime)
        {
            var dateTime = DateTime.Value.LocalDateTime;

            switch (Mode)
            {
                case DateTimeMode.DateOnly:
                {
                    return dateTime. ToShortDateString(); // d
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