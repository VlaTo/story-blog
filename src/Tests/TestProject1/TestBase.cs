using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1;

public abstract class TestBase
{
    [TestInitialize]
    public async Task TestSetup()
    {
        await ArrangeAsync();
        await ActAsync();
    }

    [TestCleanup]
    public Task TestCleanup()
    {
        return CleanupAsync();
    }

    public abstract Task ArrangeAsync();
    
    public abstract Task ActAsync();

    public abstract Task CleanupAsync();
}