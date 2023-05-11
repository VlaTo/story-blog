using System.Globalization;
using Microsoft.Extensions.Localization;

namespace StoryBlog.Web.Client.Blog.Core;

internal abstract class TimeSpanQuantity
{
    private readonly IStringLocalizer localizer;
    private readonly string category;

    protected TimeSpanQuantity(IStringLocalizer localizer, string category)
    {
        this.localizer = localizer;
        this.category = category;
    }

    public string GetQuantityString(DateTime dateTime, TimeSpan timeSpan)
    {
        var quantity = GetQuantity(timeSpan);
        var suffix = Enum.GetName(quantity)?.ToLowerInvariant();
        var format = localizer[$"{category}_{suffix}"];

        return String.Format(format, timeSpan.TotalDays, dateTime);
    }

    protected abstract Quantity GetQuantity(TimeSpan timeSpan);
    
    internal enum Quantity
    {
        Today,
        Yesterday,
        BeforeYesterday,
        Few,
        Many,
        Months,
        Years,
        Other
    }
}

internal sealed class RussianTimeSpanQuantity : TimeSpanQuantity
{
    public RussianTimeSpanQuantity(IStringLocalizer localizer, string category)
        : base(localizer, category)
    {
    }

    protected override Quantity GetQuantity(TimeSpan timeSpan)
    {
        var days = timeSpan.TotalDays;

        if (30 < days)
        {
            return 365 < days ? Quantity.Years : Quantity.Months;
        }

        if (4 < days)
        {
            return Quantity.Many;
        }

        switch (days)
        {
            case 0:
            {
                return Quantity.Today;
            }

            case 1:
            {
                return Quantity.Yesterday;
            }

            case 2:
            {
                return Quantity.BeforeYesterday;
            }

            default:
            {
                return Quantity.Few;
            }
        }
    }
}