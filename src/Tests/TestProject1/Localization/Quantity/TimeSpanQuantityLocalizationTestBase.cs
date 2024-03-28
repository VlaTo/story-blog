namespace TestProject1.Localization.Quantity;

public abstract class TimeSpanQuantityLocalizationTestBase : TestBase
{
    internal StringLocalizerStub Localizer
    {
        get; 
        set;
    } 

    public override Task ArrangeAsync()
    {
        Localizer = new StringLocalizerStub();
        return Task.CompletedTask;
    }

    public override Task CleanupAsync() => Task.CompletedTask;
}