using Allure.Net.Commons;
using Reqnroll;
using System.Text.Json;

namespace ProductCatalog.Acceptance.Tests.Features.Common
{
    [Binding]
    public sealed class TestRunHooks(ScenarioApiContext apiContext, ScenarioContext scenarioContext, FeatureContext featureContext)
    {
        private ApplicationFactory? _factory;
        private JsonDocument? _openApi;
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
            apiContext.InitializeEvidence(scenarioContext.ScenarioInfo, featureContext.FeatureInfo.Title);
            _databaseName = $"acceptance_{Guid.NewGuid():N}";
            try
            {
                _factory = new ApplicationFactory(AcceptanceSqlServer.ForDatabase(_databaseName));
                await _factory.MigrateAsync();
                apiContext.Factory = _factory;
                using (var swaggerClient = _factory.CreateClient())
                    _openApi = JsonDocument.Parse(await swaggerClient.GetStringAsync("/swagger/v1/swagger.json"));
                apiContext.OpenApi = _openApi;
                apiContext.Client = apiContext.CreateClient(_factory, validateContract: true);
            }
            catch
            {
                await CleanupAsync();
                throw;
            }
        }

        [AfterScenario]
        public Task AfterScenario() => CleanupAsync();

        [BeforeStep]
        public void BeforeStep()
        {
            apiContext.PublishEvidenceIdentity();
            var description = scenarioContext.ScenarioInfo.Description;
            if (!string.IsNullOrWhiteSpace(description))
                AllureApi.SetDescription(description);
        }

        [AfterStep]
        public void AfterStep() => apiContext.FlushEvidence();

        [AfterTestRun]
        public static Task AfterTestRun() => AcceptanceSqlServer.DisposeAsync();

        private async Task CleanupAsync()
        {
            try
            {
                apiContext.Client?.Dispose();
                _openApi?.Dispose();
                _openApi = null;
                apiContext.OpenApi = null;
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
