using Microsoft.Extensions.Localization;

namespace StoryBlog.Web.Client.Core.Localization;

public sealed class RussianTimeSpanQuantity : TimeSpanQuantity
{
    public RussianTimeSpanQuantity(IStringLocalizer localizer, string category)
        : base(localizer, category)
    {
    }

    protected override Quantity GetQuantity(TimeSpan timeSpan, out object qty)
    {
        var days = timeSpan.TotalDays;

        if (30 < days)
        {
            if (365 < days)
            {
                qty = (int)(days / 365);
                return Quantity.Years;
            }

            qty = (int)(days / 30);

            return Quantity.Months;
        }

        qty = days;

        if (4 < days)
        {
            return Quantity.Many;
        }

        return days switch
        {
            0 => Quantity.Today,
            1 => Quantity.Yesterday,
            2 => Quantity.BeforeYesterday,
            _ => Quantity.Few
        };
    }

    protected override IStringQuantity GetInnerQuantity(string category)
    {
        return new RussianQuantity(Localizer, category);
    }
}