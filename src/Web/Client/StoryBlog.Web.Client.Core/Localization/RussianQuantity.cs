using Microsoft.Extensions.Localization;

namespace StoryBlog.Web.Client.Core.Localization;

internal sealed class RussianQuantity : IStringQuantity
{
    private readonly IStringLocalizer localizer;
    private readonly string category;

    public RussianQuantity(IStringLocalizer localizer, string category)
    {
        this.localizer = localizer;
        this.category = category;
    }

    public string GetQuantityString(params object[] args)
    {
        var suffix = GetQuantitySuffix(args);
        var format = localizer[$"{category}_{suffix}"];
        return string.Format(format, args);
    }

    private string GetQuantitySuffix(object[] args)
    {
        // 1 == (x%10) месяц
        // [2-4] месяца
        // * месяцев

        if (args.Length > 1)
        {
            return "other";
        }

        var value = Convert.ToInt32(args[0]);

        if (0 == value)
        {
            return "zero";
        }

        if (1 == value)
        {
            return "one";
        }

        return (value % 10) switch
        {
            1 => 11 == value ? "many" : "one",
            2 => "few",
            3 => "few",
            4 => "few",
            _ => "many"
        };
    }
}