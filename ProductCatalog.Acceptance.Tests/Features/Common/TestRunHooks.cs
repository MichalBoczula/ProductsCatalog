using Reqnroll;

namespace ProductCatalog.Acceptance.Tests.Features.Common
{
    [Binding]
    public static class TestRunHooks
    {
        public static ApplicationFactory Factory { get; private set; } = default!;
        public static HttpClient Client { get; private set; } = default!;

        [BeforeTestRun]
        public static async Task BeforeTestRun()
        {
            Factory = new ApplicationFactory();
            await Factory.InitializeAsync();
            Client = Factory.CreateClient();
        }

        [AfterTestRun]
        public static async Task AfterTestRun()
        {
            Client.Dispose();
            await Factory.DisposeAsync();
        }
    }
}
