using Microsoft.Extensions.DependencyInjection;

namespace TestProject1.Controllers;

public abstract class TestControllers : TestBase
{
    protected ServiceCollection Services
    {
        get; 
        set;
    }

    public override Task ArrangeAsync()
    {
        Services = new ServiceCollection();
        Services.AddLogging();

        return Task.CompletedTask;
    }
}