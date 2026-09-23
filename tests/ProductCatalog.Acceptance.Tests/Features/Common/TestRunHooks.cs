using Reqnroll;

namespace ProductCatalog.Acceptance.Tests.Features.Common
{
    [Binding]
    public sealed class TestRunHooks(ScenarioApiContext apiContext)
    {
        private ApplicationFactory? _factory;
        private string? _databaseName;

        [BeforeTestRun]
        public static async Task BeforeTestRun()
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "artifacts", "allure-results", "acceptance");
            Directory.CreateDirectory(dir);
            await AcceptanceSqlServer.StartAsync();
        }

        [BeforeScenario]
        public async Task BeforeScenario()
        {
            _databaseName = $"acceptance_{Guid.NewGuid():N}";
            try
            {
                _factory = new ApplicationFactory(AcceptanceSqlServer.ForDatabase(_databaseName));
                await _factory.MigrateAsync();
                apiContext.Factory = _factory;
                apiContext.Client = _factory.CreateClient();
            }
            catch
            {
                await CleanupAsync();
                throw;
            }
        }

        [AfterScenario]
        public Task AfterScenario() => CleanupAsync();

        [AfterTestRun]
        public static Task AfterTestRun() => AcceptanceSqlServer.DisposeAsync();

        private async Task CleanupAsync()
        {
            try
            {
                apiContext.Client?.Dispose();
                if (_factory is not null)
                {
                    await _factory.DisposeAsync();
                }
            }
            finally
            {
                _factory = null;
                apiContext.Client = null;
                apiContext.Factory = null;
                if (_databaseName is not null)
                {
                    var databaseName = _databaseName;
                    _databaseName = null;
                    await AcceptanceSqlServer.DropDatabaseAsync(databaseName);
                }
            }
        }
    }
}
