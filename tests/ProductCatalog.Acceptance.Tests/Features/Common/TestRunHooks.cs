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
            EnsureAllureResultsDirectory();
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

        private static void EnsureAllureResultsDirectory()
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "artifacts", "allure-results", "acceptance");
            Directory.CreateDirectory(dir);
        }
    }
}
