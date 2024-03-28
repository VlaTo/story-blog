using System.Text.RegularExpressions;
using Microsoft.Extensions.Localization;

namespace StoryBlog.Web.Client.Core.Localization;

public abstract class TimeSpanQuantity
{
    private const string PatternGroupKey = "index";
    private const string NameGroupKey = "group_name";


    private static readonly Regex Test = new(
        @"\[\s*(?'index'\d+?)\s*,\s*""(?'group_name'\w*)""\s*\]",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled
    );

    protected IStringLocalizer Localizer
    {
        get;
    }

    protected string Category
    {
        get;
    }

    protected TimeSpanQuantity(IStringLocalizer localizer, string category)
    {
        Localizer = localizer;
        Category = category;
    }

    public string GetQuantityString(DateTime dateTime, TimeSpan timeSpan)
    {
        // TODO: сырая реализация локализованных числительных
        var quantity = GetQuantity(timeSpan, out var qty);
        var suffix = Enum.GetName(quantity)!.ToLowerInvariant();
        var pattern = Localizer[$"{Category}_{suffix}"];
        var parameters = new[] { qty, dateTime };
        var format = Test.Replace(pattern.Value, match =>
        {
            var index = Convert.ToInt32(match.Groups[PatternGroupKey].Value);
            var qnt = GetInnerQuantity(match.Groups[NameGroupKey].Value);
            return qnt.GetQuantityString(parameters[index]);
        });
        
        return String.Format(format, parameters);
    }

    protected abstract Quantity GetQuantity(TimeSpan timeSpan, out object qty);
    
    protected abstract IStringQuantity GetInnerQuantity(string localizationGroup);
    
    protected internal enum Quantity
    {
        Few,
        Many,
        Today,
        Yesterday,
        BeforeYesterday,
        Months,
        Years,
        Other
    }
}