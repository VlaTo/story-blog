using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoryBlog.Web.Client.Core.Localization;
using StoryBlog.Web.Microservices.Posts.Application.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestProject1.Localization.Quantity;

[TestClass]
public sealed class RussianTimeSpanQuantityLocalizationTests : TimeSpanQuantityLocalizationTestBase
{
    private const string DefaultLocalizationCategory = "DateTime";

    private RussianTimeSpanQuantity TimeSpanQuantity
    {
        get; 
        set;
    }

    private string QuanityString
    {
        get;
        set;
    }

    #region Arrange \ Act

    public override async Task ArrangeAsync()
    {
        await base.ArrangeAsync();

        TimeSpanQuantity = new RussianTimeSpanQuantity(Localizer, DefaultLocalizationCategory);
        Localizer.Strings[$"{DefaultLocalizationCategory}_months"] = "[0,\"Months\"] назад";
        Localizer.Strings["Months_other"] = "error";
        Localizer.Strings["Months_zero"] = "";
        Localizer.Strings["Months_one"] = "{0:D} месяц";
        Localizer.Strings["Months_few"] = "{0:D} месяца";
        Localizer.Strings["Months_many"] = "{0:D} месяцев";
    }

    public override Task ActAsync()
    {
        var dateTime = new DateTime(2024, 3, 8, 13, 04, 23);
        var elapsed = TimeSpan.FromDays(48.0d);

        QuanityString = TimeSpanQuantity.GetQuantityString(dateTime, elapsed);

        return Task.CompletedTask;
    }

    #endregion

    [TestMethod]
    public void Test()
    {
        Assert.AreEqual("48 месяцев назад", QuanityString);
    }
}