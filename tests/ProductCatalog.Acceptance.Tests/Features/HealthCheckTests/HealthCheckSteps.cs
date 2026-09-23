using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Acceptance.Tests.Features.Common;
using ProductCatalog.Infrastructure.Contexts.Commands;
using Reqnroll;
using Shouldly;

namespace ProductCatalog.Acceptance.Tests.Features.HealthCheckTests
{
    [Binding]
    public class HealthCheckSteps
    {
        private readonly ScenarioApiContext _apiContext;

        public HealthCheckSteps(ScenarioApiContext apiContext)
        {
            _apiContext = apiContext;
        }

        private HttpResponseMessage? _httpResponseMessage;

        [When("I request the health endpoint")]
        public async Task WhenIRequestTheHealthEndpoint()
        {
            _httpResponseMessage = await _apiContext.Client!.GetAsync("/health/live");
        }

        [When("I request the ready health endpoint")]
        public async Task WhenIRequestTheReadyHealthEndpoint()
        {
            _httpResponseMessage = await _apiContext.Client!.GetAsync("/health/ready");
        }

        [When("the scenario SQL database becomes unavailable")]
        public async Task WhenTheScenarioSqlDatabaseBecomesUnavailable()
        {
            using var scope = _apiContext.Factory!.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ProductsContext>();
            await context.Database.EnsureDeletedAsync();
        }

        [When("the scenario SQL database becomes available again")]
        public Task WhenTheScenarioSqlDatabaseBecomesAvailableAgain()
            => _apiContext.Factory!.MigrateAsync();

        [When("the scenario SQL history table is removed")]
        public async Task WhenTheScenarioSqlHistoryTableIsRemoved()
        {
            using var scope = _apiContext.Factory!.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ProductsContext>();
            await context.Database.ExecuteSqlRawAsync("DROP TABLE [dbo].[TB_MobilePhones_History];");
        }

        [Then("the response status code should be {int}")]
        public void ThenTheResponseStatusCodeShouldBe(int p0)
        {
            _httpResponseMessage!.StatusCode.ShouldBe((System.Net.HttpStatusCode)p0);
        }
    }
}
